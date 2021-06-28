using Models.State.Board;
using UnityEngine;
using View.Prefab.Spawners;

namespace View.Prefab.Factories
{
    public class TileFactory
    {
        private readonly TileSpawner.Factory _tileFactory;

        public TileFactory(TileSpawner.Factory tileFactory)
        {
            _tileFactory = tileFactory;
        }

        public void CreateTile(Position position, GameObject parent, Color32 colour)
        {
            _tileFactory.Create(position, parent, colour);
        }
    }
}