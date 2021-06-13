using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.MoveGenerators
{
    public class PossibleTurnMovesGenerator : IAllPossibleMovesGenerator
    {
        private readonly IBoardInfo _boardInfo;
        private readonly PinnedPieceFilter _pinnedPieceFilter;

        public PossibleTurnMovesGenerator(IBoardInfo boardInfo, PinnedPieceFilter pinnedPieceFilter)
        {
            _boardInfo = boardInfo;
            _pinnedPieceFilter = pinnedPieceFilter;
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
        public IDictionary<Position, HashSet<Position>> GetPossibleMoves(BoardState boardState,
            PieceColour turn)
        {
            // board info will mess with concurrent execution because it is stateful and not instantiated 
            // for each call 
            _boardInfo.EvaluateBoard(boardState, turn);
            var turnMoves = _boardInfo.TurnMoves;
            var nonTurnMoves = _boardInfo.NonTurnMoves;
            var kingPosition = _boardInfo.KingPosition; // will be set to 8,8 by default if no king present (as null)

            var checkManager = new CheckedStateManager(boardState);

            checkManager.EvaluateCheck(nonTurnMoves, kingPosition);
            if (checkManager.IsCheck)
                checkManager.UpdatePossibleMovesWhenInCheck(_boardInfo);
            else
                KingMoveFilter.RemoveNonTurnMovesFromKingMoves(turnMoves, nonTurnMoves, kingPosition);

            _pinnedPieceFilter.FilterMoves(_boardInfo, boardState);
            return turnMoves;
        }
    }
}