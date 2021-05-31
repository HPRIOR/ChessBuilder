using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class AllPossibleMovesGenerator : IAllPossibleMovesGenerator
    {
        private readonly IBoardEval _boardEval;
        private readonly KingMoveFilter _kingMoveFilter;

        public AllPossibleMovesGenerator(IPossibleMoveFactory possibleMoveFactory, IBoardEval boardEval)
        {
            _possibleMoveFactory = possibleMoveFactory;
            _boardEval = boardEval;
            _kingMoveFilter = kingMoveFilter;
        }


        public IDictionary<BoardPosition, HashSet<BoardPosition>> GetPossibleMoves(BoardState boardState,
            PieceColour turn)
        {
            _boardEval.EvaluateBoard(boardState, turn);
            var turnMoves = _boardEval.TurnMoves;
            var nonTurnMoves = _boardEval.NonTurnMoves;
            var kingPosition = _boardEval.KingPosition;

            var checkedState = new CheckedState(boardState, previousMove, _possibleMoveFactory);
            checkedState.EvaluateCheck(nonTurnMoves, kingPosition);
            if (checkedState.IsTrue)
            {
                turnMoves = checkedState.PossibleMovesWhenInCheck(turnMoves, nonTurnMoves, kingPosition);
            }
            else
            {
                if (!kingPosition.Equals(new BoardPosition(8, 8))) // using out of bounds as null
                    _kingMoveFilter.RemoveNonTurnMovesFromKingMoves(turnMoves, nonTurnMoves, kingPosition, boardState);
            }

            // find king moves
            // find pinned pieces
            return turnMoves;
        }
    }
}