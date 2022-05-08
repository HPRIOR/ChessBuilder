using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using UnityEngine;
using View.Prefab.Spawners;

namespace View.Prefab.Factories
{
    public class TileFactory
    {
        private readonly TileSpawner.Factory _tileFactory;
        private readonly GameContext _context;
        private readonly IGameStateController _gameStateController;


        public TileFactory(TileSpawner.Factory tileFactory, GameContext context, IGameStateController gameStateController)
        {
            _tileFactory = tileFactory;
            _context = context;
            _gameStateController = gameStateController;
        }

        public void CreateTile(Position position, GameObject parent, Color32 colour)
        {
            var tile = _tileFactory.Create(position, parent, colour);
            // TODO delete RightClickBuild if not players context
        }
    }
}