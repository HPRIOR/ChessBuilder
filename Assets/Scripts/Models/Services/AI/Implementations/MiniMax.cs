using System;
using Models.Services.AI.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;
using UnityEngine.Rendering;

namespace Models.Services.AI
{
    public class MiniMax
    {
        private readonly IAiCommandGenerator _aiCommandGenerator;
        private readonly IStaticEvaluator _staticEvaluator;

        public MiniMax(IStaticEvaluator staticEvaluator, IAiCommandGenerator aiCommandGenerator)
        {
            _staticEvaluator = staticEvaluator;
            _aiCommandGenerator = aiCommandGenerator;
        }

        public (Func<BoardState, PieceColour, GameState> move, int score) GetMaximizingTurn(
            GameState gameState,
            int depth, 
            PieceColour turn)
        {
            if (depth == 0 || gameState.CheckMate) return (null, _staticEvaluator.Evaluate(gameState).GetPoints(turn));

            // initialise best move placeholders
            Func<BoardState, PieceColour, GameState> bestMove = null;
            var bestScore = int.MinValue;

            // iterate through all moves
            var moves = _aiCommandGenerator.GenerateCommands(gameState);
            foreach (var move in moves)
            {
                // get updated board state
                var newGameState = move(gameState.BoardState, turn);

                // 'bubble up' score 
                var (_, recurseScore) =
                    GetMaximizingTurn(newGameState, depth - 1, turn);
                var currentScore = -recurseScore;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = move;
                }
            }

            return (bestMove, bestScore);
            
        }
    }
}