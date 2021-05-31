using System.Collections.Generic;
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

        public AllPossibleMovesGenerator(IBoardEval boardEval, KingMoveFilter kingMoveFilter)
        {
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

            var checkManager = new CheckedStateManager(boardState, _kingMoveFilter);
            checkManager.EvaluateCheck(nonTurnMoves, kingPosition);
            if (checkManager.IsCheck)
            {
                checkManager.UpdatePossibleMovesWhenInCheck(turnMoves, nonTurnMoves, kingPosition);
            }
            else
            {
                if (!kingPosition.Equals(new BoardPosition(8, 8))) // using out of bounds as null
                    _kingMoveFilter.RemoveNonTurnMovesFromKingMoves(turnMoves, nonTurnMoves, kingPosition, boardState);
            }

            // find pinned pieces
            return turnMoves;
        }
    }
}