using System;
using System.Threading.Tasks;
using Models.Services.AI.Interfaces;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;

namespace Models.Services.AI.Implementations
{
    public class AiMoveGenerator
    {
        private const int WindowSize = 3000;
        private readonly IAiPossibleMoveGenerator _aiPossibleMoveGenerator;
        private readonly IStaticEvaluator _staticEvaluator;

        public AiMoveGenerator(IStaticEvaluator staticEvaluator, IAiPossibleMoveGenerator aiPossibleMoveGenerator)
        {
            _staticEvaluator = staticEvaluator;
            _aiPossibleMoveGenerator = aiPossibleMoveGenerator;
        }

        public Task<Func<GameState, PieceColour, GameState>> GetMove(
            GameState gameState,
            int depth,
            PieceColour turn)
        {
            return Task.Run(() =>
            {
                const int alpha = int.MinValue;
                const int beta = int.MaxValue;

                var (move, _) = NegaScout(gameState, depth, 0, turn, alpha, beta);
                return move;
            });
        }

        private (Func<GameState, PieceColour, GameState> move, int score) NegaScout(
            GameState gameState,
            int maxDepth,
            int currentDepth,
            PieceColour turn,
            int alpha,
            int beta)
        {
            if (currentDepth == maxDepth || gameState.CheckMate)
                return (null, _staticEvaluator.Evaluate(gameState).GetPoints(turn));

            // initialise best move placeholders
            Func<GameState, PieceColour, GameState> bestMove = null;
            var bestScore = int.MinValue;
            var adaptiveBeta = beta;

            // iterate through all moves
            var moves = _aiPossibleMoveGenerator.GenerateMoves(gameState);
            foreach (var move in moves)
            {
                // get updated board state
                var newGameState = move(gameState, turn);

                // recurse
                var (_, recurseScore) = NegaScout(newGameState, maxDepth, currentDepth + 1, turn.NextTurn(),
                    -adaptiveBeta, -Math.Max(alpha, bestScore));
                var currentScore = -recurseScore;

                if (currentScore > bestScore)
                {
                    if (adaptiveBeta == beta || currentDepth >= maxDepth - 2)
                    {
                        bestScore = currentScore;
                        bestMove = move;
                    }
                    else
                    {
                        var (negativeBestMove, negativeBestScore) = NegaScout(
                            newGameState, maxDepth, currentDepth, turn.NextTurn(), -beta, -currentScore
                        );
                        bestScore = -negativeBestScore;
                        bestMove = move;
                    }

                    if (bestScore >= beta) return (bestMove, bestScore);

                    adaptiveBeta = Math.Max(alpha, bestScore) + 1;
                }
            }

            return (bestMove, bestScore);
        }
    }
}