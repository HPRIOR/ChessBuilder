using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class CheckedState : ICheckedState
    {
        private readonly BoardState _boardState;

        private readonly HashSet<BoardPosition> _positionsBetweenKingAndCheckPiece;
        private readonly IPossibleMoveFactory _possibleMoveFactory;
        private readonly BoardPosition _previousMove;
        private readonly PieceColour _turn;
        private BoardPosition _kingPosition;

        public CheckedState(
            BoardState boardState,
            BoardPosition previousMove,
            PieceColour turn,
            IPossibleMoveFactory possibleMoveFactory)
        {
            _boardState = boardState;
            _previousMove = previousMove;
            _turn = turn;
            _possibleMoveFactory = possibleMoveFactory;

            // check for condition and return pieces between checked king and checking piece
            var checkedPositions = GetPositionsBetweenCheckedKing();
            _positionsBetweenKingAndCheckPiece = checkedPositions;
            IsTrue = checkedPositions.Any();
        }

        public bool IsTrue { get; }

        public IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleNonKingMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> possibleMoves)
        {
            foreach (var keyVal in possibleMoves)
            {
                var notKingPiece = !keyVal.Key.Equals(_kingPosition);
                if (notKingPiece) keyVal.Value.IntersectWith(_positionsBetweenKingAndCheckPiece);
            }

            return possibleMoves;
        }

        private HashSet<BoardPosition> GetPositionsBetweenCheckedKing()
        {
            var movedPiece = _boardState.Board[_previousMove.X, _previousMove.Y].CurrentPiece;
            var possibleMoves =
                _possibleMoveFactory
                    .GetPossibleMoveGenerator(movedPiece)
                    .GetPossiblePieceMoves(_previousMove, _boardState);
            foreach (var boardPosition in possibleMoves)
            {
                var pieceAtBoardPosition = _boardState.Board[boardPosition.X, boardPosition.Y].CurrentPiece;

                var kingPosition = pieceAtBoardPosition.Colour != movedPiece.Colour &&
                                   (pieceAtBoardPosition.Type == PieceType.BlackKing ||
                                    pieceAtBoardPosition.Type == PieceType.WhiteKing);
                if (kingPosition)
                {
                    _kingPosition = boardPosition;
                    var result = ScanPositionGenerator.GetPositionsBetween(boardPosition, _previousMove)
                        .Concat(new List<BoardPosition> {_previousMove});
                    return new HashSet<BoardPosition>(result);
                }
            }

            return new HashSet<BoardPosition>();
        }
    }
}