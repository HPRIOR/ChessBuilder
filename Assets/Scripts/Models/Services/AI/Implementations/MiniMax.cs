using System;
using Models.Services.AI.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public class MiniMax
    {
        private const int WindowSize = 3000;
        private readonly IAiMoveGenerator _aiMoveGenerator;
        private readonly IStaticEvaluator _staticEvaluator;

        public MiniMax(IStaticEvaluator staticEvaluator, IAiMoveGenerator aiMoveGenerator)
        {
            _staticEvaluator = staticEvaluator;
            _aiMoveGenerator = aiMoveGenerator;
        }

        public Func<BoardState, PieceColour, GameState> GetMove(
            GameState gameState,
            int depth,
            PieceColour turn)
        {
            const int alpha = int.MinValue;
            const int beta = int.MaxValue;

            var (move, _) = NegaScout(gameState, depth, 0, turn, alpha, beta);
            return move;
        }

        public (Func<BoardState, PieceColour, GameState> move, int score) NegaScout(
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
            Func<BoardState, PieceColour, GameState> bestMove = null;
            var bestScore = int.MinValue;
            var adaptiveBeta = beta;

            // iterate through all moves
            var moves = _aiMoveGenerator.GenerateMoves(gameState);
            foreach (var move in moves)
            {
                // get updated board state
                var newGameState = move(gameState.BoardState, turn);

                // recurse
                var (_, recurseScore) = NegaScout(newGameState, maxDepth, currentDepth + 1, turn,
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
                            newGameState, maxDepth, currentDepth, turn, -beta, -currentScore
                        );
                        bestScore = -negativeBestScore;
                        bestMove = negativeBestMove;
                    }

                    if (bestScore >= beta) return (bestMove, bestScore);

                    adaptiveBeta = Math.Max(alpha, bestScore) + 1;
                }
            }

            return (bestMove, bestScore);
        }
    }
}