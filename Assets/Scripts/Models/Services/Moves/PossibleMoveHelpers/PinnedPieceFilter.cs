using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.BoardPos;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class PinnedPieceFilter
    {
        private readonly HashSet<PieceType> _scanningPieces = new HashSet<PieceType>
        {
            PieceType.BlackBishop, PieceType.BlackQueen, PieceType.BlackRook, PieceType.WhiteBishop,
            PieceType.WhiteQueen, PieceType.WhiteRook
        };


        public void FilterMoves(IBoardInfo boardInfo, BoardState boardState)
        {
            var turnMoves = boardInfo.TurnMoves;
            var nonTurnScanningMoves = GetScanningPieces(boardInfo.NonTurnMoves, boardState);
            var kingPosition = boardInfo.KingPosition;
            var turnPieces = new HashSet<BoardPosition>(turnMoves.Keys);
            turnPieces.Remove(kingPosition);
            foreach (var moves in nonTurnScanningMoves)
            {
                var turnPiecePositionIntersect = moves.Value.Intersect(turnPieces).ToList(); // One or none
                var nonTurnMovesContainTurnPiece = turnPiecePositionIntersect.Any();

                if (
                    nonTurnMovesContainTurnPiece &&
                    DirectionOfPinPointsToKing(kingPosition, turnPiecePositionIntersect.First(), moves.Key) &&
                    TheNextPieceIsKing(moves.Key, turnPiecePositionIntersect.First(), kingPosition, boardState)
                )
                    turnMoves[turnPiecePositionIntersect.First()].IntersectWith(PossibleEscapeMoves(moves));
            }
        }

        private static HashSet<BoardPosition> PossibleEscapeMoves(
            KeyValuePair<BoardPosition, HashSet<BoardPosition>> moves)
        {
            var pinningMoves = new HashSet<BoardPosition>
            {
                moves.Key
            };
            pinningMoves.UnionWith(moves.Value);
            return pinningMoves;
        }

        private static bool DirectionOfPinPointsToKing(BoardPosition kingPosition, BoardPosition pinnedPosition,
            BoardPosition pinningPosition)
        {
            var pinDirection = pinningPosition.DirectionTo(pinnedPosition);
            var pinnedToKingDirection = pinnedPosition.DirectionTo(kingPosition);
            return pinDirection == pinnedToKingDirection;
        }

        private static bool ContainsNonKingPiece(BoardState boardState, BoardPosition kingPosition,
            BoardPosition targetPosition)
        {
            var (x, y) = (targetPosition.X, targetPosition.Y);
            return !targetPosition.Equals(kingPosition) &&
                   boardState.Board[x, y].CurrentPiece.Type != PieceType.NullPiece;
        }


        private static bool TheNextPieceIsKing(BoardPosition scanningPiecePosition, BoardPosition turnPiecePosition,
            BoardPosition kingPosition, BoardState boardState)
        {
            var scannedBoardPositions =
                scanningPiecePosition.Scan(scanningPiecePosition.DirectionTo(turnPiecePosition));
            foreach (var scannedBoardPosition in scannedBoardPositions)
            {
                if (scannedBoardPosition.Equals(kingPosition)) return true;

                if (ContainsNonKingPiece(boardState, kingPosition,
                    scannedBoardPosition)) // escape early if piece obstructs possible king 
                    return false;
            }

            return false;
        }

        private bool PieceIsScanner(KeyValuePair<BoardPosition, HashSet<BoardPosition>> pieceMoves,
            BoardState boardState)
        {
            var pieceAtBoardPosition = boardState.Board[pieceMoves.Key.X, pieceMoves.Key.Y].CurrentPiece.Type;
            return _scanningPieces.Contains(pieceAtBoardPosition);
        }

        private IDictionary<BoardPosition, HashSet<BoardPosition>> GetScanningPieces(
            IDictionary<BoardPosition, HashSet<BoardPosition>> moves, BoardState boardState) =>
            moves
                .Where(keyVal => PieceIsScanner(keyVal, boardState))
                .ToDictionary(keyVal => keyVal.Key, keyVal => keyVal.Value);
    }
}