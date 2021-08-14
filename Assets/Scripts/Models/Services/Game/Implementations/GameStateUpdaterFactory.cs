using Models.Services.Game.Interfaces;
using Models.State.GameState;

namespace Models.Services.Game.Implementations
{
    public class GameStateUpdaterFactory
    {
        private readonly GameStateUpdater.Factory _gameStateUpdaterFactory;

        public GameStateUpdaterFactory(GameStateUpdater.Factory gameStateUpdaterFactory)
        {
            _gameStateUpdaterFactory = gameStateUpdaterFactory;
        }

        public IGameStateUpdater Create(GameState gameState) => _gameStateUpdaterFactory.Create(gameState);
    }
}