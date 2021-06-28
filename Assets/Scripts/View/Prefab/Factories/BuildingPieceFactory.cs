using Models.State.Board;
using Models.State.BuildState;
using View.Prefab.Spawners;

namespace View.Prefab.Factories
{
    public class BuildingPieceFactory
    {
        private readonly BuildingPieceSpawner.Factory _pieceFactory;

        public BuildingPieceFactory(BuildingPieceSpawner.Factory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public void Create(BuildState buildState, Position position)
        {
            _pieceFactory.Create(position, buildState);
        }
    }
}