using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class CheckedState : ICheckedState
    {
        private static readonly HashSet<PieceType> _scanningPieces = new HashSet<PieceType>
        {
            PieceType.BlackRook,
            PieceType.WhiteRook,
            PieceType.BlackQueen,
            PieceType.WhiteQueen,
            PieceType.BlackBishop,
            PieceType.WhiteBishop
        };

        private readonly BoardState _boardState;

        private readonly HashSet<BoardPosition> _positionsBetweenKingAndCheckPiece;
        private readonly IPossibleMoveFactory _possibleMoveFactory;
        private readonly BoardPosition _previousMove;


        // TODO 1. inject possible move factory to this; 2. Check all non-turn moves for check; 3. Inject into PossibleMoveGen;
        public CheckedState(BoardState boardState,
            BoardPosition previousMove,
            IPossibleMoveFactory possibleMoveFactory,
            BoardPosition kingPosition)
        {
            _boardState = boardState;
            _previousMove = previousMove;
            _possibleMoveFactory = possibleMoveFactory;

            // check for condition and return pieces between checked king and checking piece
            var checkedPositions = GetPositionsBetweenCheckedKing(kingPosition);
            _positionsBetweenKingAndCheckPiece = checkedPositions;
            IsTrue = checkedPositions.Any();
        }

        public bool IsTrue { get; }

        // in the case of a double check exit out here early and return empty possible moves aside from king moves
        public IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleNonKingMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> possibleMoves, BoardPosition kingPosition)
        {
            foreach (var keyVal in possibleMoves)
            {
                var notKingPiece = !keyVal.Key.Equals(kingPosition);
                if (notKingPiece) keyVal.Value.IntersectWith(_positionsBetweenKingAndCheckPiece);
            }

            return possibleMoves;
        }

        public IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleKingMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            var checkingPiece = _boardState.Board[_previousMove.X, _previousMove.Y].CurrentPiece;
            // check if checking piece is scanning type 
            if (_scanningPieces.Contains(checkingPiece.Type))
            {
                // remove all possible moves from king moves as well as extended positions for scanning types 
            }

            // if not, remove all possible moves from king moves
            foreach (var nonTurnMove in nonTurnMoves)
            {
                var kingMoves = turnMoves[kingPosition];
                turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(nonTurnMove.Value));
            }

            return turnMoves;
        }

        //TODO new abstraction: manipulateDictionaries of board position -> hashset board position
        // except, intersect, 

        // this will need to change when all non turn moves are evaluated for check, to catch discovered check, and double check
        // if two checks are found exit early and return an empty hashset - this will intersect with all moves producing no moves
        // hence only the king will be able to move
        private HashSet<BoardPosition> GetPositionsBetweenCheckedKing(BoardPosition kingPosition)
        {
            var movedPiece = _boardState.Board[_previousMove.X, _previousMove.Y].CurrentPiece;
            var possibleMoves =
                _possibleMoveFactory
                    .GetPossibleMoveGenerator(movedPiece)
                    .GetPossiblePieceMoves(_previousMove, _boardState);

            foreach (var boardPosition in possibleMoves)
                if (kingPosition.Equals(boardPosition))
                {
                    var result = ScanPositionGenerator.GetPositionsBetween(boardPosition, _previousMove)
                        .Concat(new List<BoardPosition> {_previousMove});
                    return new HashSet<BoardPosition>(result);
                }

            return new HashSet<BoardPosition>();
        }
    }
}