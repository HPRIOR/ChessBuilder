using System;
using Models.Services.AI.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;

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

        public (Func<BoardState, PieceColour, GameState> move, int score) GetMaximizingTurn(GameState gameState,
            int depth, PieceColour turn, bool maxiPlayer)
        {
            if (depth == 0 || gameState.CheckMate) return (null, _staticEvaluator.Evaluate(gameState).GetPoints(turn));

            if (maxiPlayer)
            {
                // will need to separate out the best move and max eval otherwise the move will be changed regardless
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
                    minEval = (move, Math.Min(eval, minEval.score));
                }

                return minEval;
            }
        }
    }
}