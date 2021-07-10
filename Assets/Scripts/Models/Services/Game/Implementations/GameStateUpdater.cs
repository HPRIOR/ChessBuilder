using System;
using Models.Services.Build.Interfaces;
using Models.Services.Game.Interfaces;
using Models.Services.Interfaces;
using Models.State;
using Models.State.Board;

namespace Models.Services.Game.Implementations
{
    public class GameStateUpdater : IGameStateUpdater
    {
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IBuildPointsCalculator _buildPointsCalculator;
        private readonly IBuildResolver _buildResolver;
        private readonly IGameOverEval _gameOverEval;
        private readonly IMovesGenerator _movesGenerator;

        public GameStateUpdater(IMovesGenerator movesGenerator, IBuildMoveGenerator buildMoveGenerator,
            IBuildPointsCalculator buildPointsCalculator, IBuildResolver buildResolver, IGameOverEval gameOverEval)
        {
            _movesGenerator = movesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _buildPointsCalculator = buildPointsCalculator;
            _buildResolver = buildResolver;
            _gameOverEval = gameOverEval;
        }

        public GameState UpdateGameState(BoardState newBoardState) => throw new NotImplementedException();
    }
}