﻿using System.Collections.Generic;
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
        private readonly IPossibleMoveFactory _possibleMoveFactory;

        public AllPossibleMovesGenerator(IPossibleMoveFactory possibleMoveFactory, IBoardEval boardEval)
        {
            _possibleMoveFactory = possibleMoveFactory;
            _boardEval = boardEval;
        }


        public IDictionary<BoardPosition, HashSet<BoardPosition>> GetPossibleMoves(BoardState boardState,
            PieceColour turn, BoardPosition previousMove)
        {
            _boardEval.EvaluateBoard(boardState, turn);
            var turnMoves = _boardEval.TurnMoves;
            var nonTurnMoves = _boardEval.NonTurnMoves;
            var kingPosition = _boardEval.KingPosition;

            var checkedState = new CheckedState(boardState, previousMove, turn, _possibleMoveFactory);
            if (checkedState.IsTrue)
                turnMoves =
                    (Dictionary<BoardPosition, HashSet<BoardPosition>>) checkedState.PossibleNonKingMovesWhenInCheck(
                        turnMoves);

            if (!kingPosition.Equals(new BoardPosition(8, 8))) // using out of bounds as null
                turnMoves = IntersectKingMovesWithNonTurnMoves(nonTurnMoves, turnMoves, kingPosition);

            // find king moves
            // find pinned pieces
            return turnMoves;
        }


        private IDictionary<BoardPosition, HashSet<BoardPosition>> IntersectKingMovesWithNonTurnMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            BoardPosition kingPosition)
        {
            foreach (var keyVal in nonTurnMoves)
            {
                var kingMoves = turnMoves[kingPosition];
                turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(keyVal.Value));
            }

            return turnMoves;
        }
    }
}