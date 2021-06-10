using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class PossibleTurnMovesGenerator : IAllPossibleMovesGenerator
    {
        private readonly IBoardEval _boardEval;
        private readonly KingMoveFilter _kingMoveFilter;

        public PossibleTurnMovesGenerator(IBoardEval boardEval, KingMoveFilter kingMoveFilter)
        {
            _boardEval = boardEval;
            _kingMoveFilter = kingMoveFilter;
        }

        /// <summary>
        ///     Generates all the possible moves for a given player given the state of the board.
        /// </summary>
        /// <remarks>
        ///     This works by calculating all the possible moves for turn and non-turn pieces. Turn moves include opposing
        ///     pieces, and preclude friendly pieces. Non turn Moves include both opposing pieces and friendly pieces.
        ///     If check occurs, then only those moves between the king and the checking piece are allowed for non-king pieces
        ///     and a move away from check for the king piece. King moves are filtered by non turn moves. This is why non-turn
        ///     moves include friendly pieces, as this constitutes a protected piece which the king cannot take.
        /// </remarks>
        /// <param name="boardState"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
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