using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.BoardPos;

namespace Models.Services.Moves.MoveHelpers
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
            var turnPiecePosition = new HashSet<Position>(turnMoves.Keys);
            turnPiecePosition.Remove(kingPosition);
            foreach (var nonTurnMoves in nonTurnScanningMoves)
            {
                var turnPiecePositionIntersect =
                    nonTurnMoves.Value.Intersect(turnPiecePosition).ToList(); // One or none
                var nonTurnMovesContainTurnPiece = turnPiecePositionIntersect.Any();

                if (
                    nonTurnMovesContainTurnPiece &&
                    // DirectionOfPinPointsToKing(kingPosition, turnPiecePositionIntersect.First(), moves.Key) &&
                    TheNextPieceIsKing(nonTurnMoves.Key, turnPiecePositionIntersect.First(), kingPosition, boardState)
                )
                    turnMoves[turnPiecePositionIntersect.First()].IntersectWith(PossibleEscapeMoves(nonTurnMoves));
            }
        }

        private static HashSet<Position> PossibleEscapeMoves(
            KeyValuePair<Position, HashSet<Position>> moves)
        {
            var pinningMoves = new HashSet<Position>
            {
                moves.Key
            };
            pinningMoves.UnionWith(moves.Value);
            return pinningMoves;
        }

        private static bool DirectionOfPinPointsToKing(Position kingPosition, Position pinnedPosition,
            Position pinningPosition)
        {
            var pinDirection = pinningPosition.DirectionTo(pinnedPosition);
            var pinnedToKingDirection = pinnedPosition.DirectionTo(kingPosition);
            return pinDirection == pinnedToKingDirection;
        }

        private static bool ContainsNonKingPiece(BoardState boardState, Position kingPosition,
            Position targetPosition)
        {
            var (x, y) = (targetPosition.X, targetPosition.Y);
            return !targetPosition.Equals(kingPosition) &&
                   boardState.Board[x, y].CurrentPiece.Type != PieceType.NullPiece;
        }


        private static bool TheNextPieceIsKing(Position scanningPiecePosition, Position turnPiecePosition,
            Position kingPosition, BoardState boardState)
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

        private bool PieceIsScanner(KeyValuePair<Position, HashSet<Position>> pieceMoves,
            BoardState boardState)
        {
            var pieceAtBoardPosition = boardState.Board[pieceMoves.Key.X, pieceMoves.Key.Y].CurrentPiece.Type;
            return _scanningPieces.Contains(pieceAtBoardPosition);
        }

        private IDictionary<Position, HashSet<Position>> GetScanningPieces(
            IDictionary<Position, HashSet<Position>> moves, BoardState boardState) =>
            moves
                .Where(keyVal => PieceIsScanner(keyVal, boardState))
                .ToDictionary(keyVal => keyVal.Key, keyVal => keyVal.Value);
    }
}