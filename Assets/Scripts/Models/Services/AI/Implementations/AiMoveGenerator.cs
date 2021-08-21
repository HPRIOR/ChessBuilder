using System;
using Models.Services.AI.Interfaces;
using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public class AiMoveGenerator
    {
        private const int WindowSize = 3000;
        private readonly IAiPossibleMoveGenerator _aiPossibleMoveGenerator;
        private readonly GameStateUpdaterFactory _gameStateUpdaterFactory;
        private readonly IStaticEvaluator _staticEvaluator;

        public AiMoveGenerator(IStaticEvaluator staticEvaluator, IAiPossibleMoveGenerator aiPossibleMoveGenerator,
            GameStateUpdaterFactory gameStateUpdaterFactory)
        {
            _staticEvaluator = staticEvaluator;
            _aiPossibleMoveGenerator = aiPossibleMoveGenerator;
            _gameStateUpdaterFactory = gameStateUpdaterFactory;
        }

        public Action<PieceColour, IGameStateUpdater> GetMove(
            GameState gameState,
            int depth,
            PieceColour turn)
        {
            const int alpha = int.MinValue;
            const int beta = int.MaxValue;

            // need to pass in a copy of the game state
            var (move, _) = NegaScout(_gameStateUpdaterFactory.Create(gameState.Clone() as GameState), depth, 0, turn,
                alpha, beta);
            return move;
        }

        private (Action<PieceColour, IGameStateUpdater> move, int score) NegaScout(
            IGameStateUpdater gameStateUpdater,
            int maxDepth,
            int currentDepth,
            PieceColour turn,
            int alpha,
            int beta)
        {
            if (currentDepth == maxDepth || gameStateUpdater.GameState.CheckMate)
            {
                var boardEval = _staticEvaluator.Evaluate(gameStateUpdater.GameState).GetPoints(turn);
                gameStateUpdater.RevertGameState();
                return (null, boardEval);
            }

            // initialise best move placeholders
            Action<PieceColour, IGameStateUpdater> bestMove = null;
            var bestScore = int.MinValue;
            var adaptiveBeta = beta;

            // iterate through all moves
            var moves = _aiPossibleMoveGenerator.GenerateMoves(gameStateUpdater.GameState);
            foreach (var move in moves)
            {
                // get updated board state
                move(turn, gameStateUpdater);

                // recurse
                var (_, recurseScore) = NegaScout(gameStateUpdater, maxDepth, currentDepth + 1, turn,
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
                            gameStateUpdater, maxDepth, currentDepth, turn, -beta, -currentScore
                        );
                        bestScore = -negativeBestScore;
                        bestMove = move;
                    }

                    if (bestScore >= beta)
                    {
                        gameStateUpdater.RevertGameState();
                        return (bestMove, bestScore);
                    }

                    adaptiveBeta = Math.Max(alpha, bestScore) + 1;
                }
            }

            gameStateUpdater.RevertGameState();
            return (bestMove, bestScore);
        }
    }
}