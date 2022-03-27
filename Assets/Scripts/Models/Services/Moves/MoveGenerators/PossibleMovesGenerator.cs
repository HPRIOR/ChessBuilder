﻿using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.MoveState;
using Models.State.PieceState;

namespace Models.Services.Moves.MoveGenerators
{
    public sealed class MovesGenerator : IMovesGenerator
    {
        private readonly IBoardInfo _boardInfo;
        private readonly CheckedStateManager _checkedStateManager;
        private readonly PinnedPieceFilter _pinnedPieceFilter;

        public MovesGenerator(IBoardInfo boardInfo, PinnedPieceFilter pinnedPieceFilter,
            CheckedStateManager checkedStateManager)
        {
            _boardInfo = boardInfo;
            _pinnedPieceFilter = pinnedPieceFilter;
            _checkedStateManager = checkedStateManager;
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
        public MoveState GetPossibleMoves(BoardState boardState, PieceColour turn)
        {
            _boardInfo.EvaluateBoard(boardState, turn);
            var turnMoves = _boardInfo.TurnMoves;
            var enemyMoves = _boardInfo.EnemyMoves;
            var kingPosition = _boardInfo.KingPosition; // will be set to 8,8 by default if no king present (as null)


            _checkedStateManager.EvaluateCheck(enemyMoves, kingPosition);
            if (_checkedStateManager.IsCheck)
                _checkedStateManager.UpdatePossibleMovesWhenInCheck(_boardInfo, boardState);
            else
                KingMoveFilter.RemoveEnemyMovesFromKingMoves(turnMoves, enemyMoves, kingPosition);

            _pinnedPieceFilter.FilterMoves(_boardInfo, boardState);
            return new MoveState(turnMoves, _checkedStateManager.IsCheck);
        }
    }
}