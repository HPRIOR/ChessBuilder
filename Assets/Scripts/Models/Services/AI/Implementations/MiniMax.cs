using System;
using Models.Services.AI.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;
using Zenject;

namespace Models.Services.AI
{
    public class MiniMax
    {
        private readonly IStaticEvaluator _staticEvaluator;
        private readonly IAiCommandGenerator _aiCommandGenerator;

        public MiniMax(IStaticEvaluator staticEvaluator, IAiCommandGenerator aiCommandGenerator)
        {
            _staticEvaluator = staticEvaluator;
            _aiCommandGenerator = aiCommandGenerator;
        }

        public (Func<BoardState, PieceColour, GameState> move, int score) GetMaximizingTurn(GameState gameState,
            int depth, PieceColour turn, bool maxiPlayer)
        {
            if (depth == 0 || gameState.CheckMate)
            {
                return (null, _staticEvaluator.Evaluate(gameState).GetPoints(turn));
            }

            if (maxiPlayer)
            {
                (Func<BoardState, PieceColour, GameState> move, int score) maxEval = (null, int.MinValue);
                var moves = _aiCommandGenerator.GenerateCommands(gameState);
                foreach (var move in moves)
                {
                    var eval = GetMaximizingTurn(
                        move(gameState.BoardState, turn),
                        depth - 1,
                        turn.NextTurn(),
                        false).score;
                    maxEval = (move, Math.Max(eval, maxEval.score));
                }

                return maxEval;
            }
            else
            {
                (Func<BoardState, PieceColour, GameState> move, int score) minEval = (null, int.MaxValue);
                var moves = _aiCommandGenerator.GenerateCommands(gameState);
                foreach (var move in moves)
                {
                    var eval = GetMaximizingTurn(
                        move(gameState.BoardState, turn),
                        depth - 1,
                        turn.NextTurn(),
                        true).score;
                    minEval = (move, Math.Max(eval, minEval.score));
                }

                return minEval;
            }
        }
    }
}