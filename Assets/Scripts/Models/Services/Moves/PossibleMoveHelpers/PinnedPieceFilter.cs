using System.Collections.Generic;
using System.Linq;
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


        public void FilterMoves(BoardInfo boardInfo, BoardState boardState)
        {
            var turnMoves = boardInfo.TurnMoves;
            var nonTurnScanningMoves = GetScanningPieces(boardInfo.NonTurnMoves, boardState);
            var kingPosition = boardInfo.KingPosition;
            var turnPieces = new HashSet<BoardPosition>(turnMoves.Keys);

            foreach (var moves in nonTurnScanningMoves)
            {
                var turnPiecePosition = moves.Value.Intersect(turnPieces).ToList(); // One or none
                var nonTurnMoveContainsTurnPiece = turnPiecePosition.Any();
                if (nonTurnMoveContainsTurnPiece &&
                    TheNextPieceIsKing(moves.Key, turnPiecePosition.First(), kingPosition, boardState))
                    turnMoves[turnPiecePosition.First()].Clear();
            }
        }

        private bool ContainsNonKingPiece(BoardState boardState, BoardPosition kingPosition,
            BoardPosition targetPosition)
        {
            var (x, y) = (targetPosition.X, targetPosition.Y);
            return !targetPosition.Equals(kingPosition) &&
                   boardState.Board[x, y].CurrentPiece.Type != PieceType.NullPiece;
        }


        private bool TheNextPieceIsKing(BoardPosition scanningPiecePosition, BoardPosition turnPiecePosition,
            BoardPosition kingPosition, BoardState boardState)
        {
            var scannedBoardPositions =
                scanningPiecePosition.Scan(scanningPiecePosition.DirectionTo(turnPiecePosition));
            foreach (var scannedBoardPosition in scannedBoardPositions)
                return !ContainsNonKingPiece(boardState, kingPosition, scannedBoardPosition);
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