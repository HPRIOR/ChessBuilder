using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using View.Utils.Prefab.Spawners;

namespace View.Utils.Prefab.Factories
{
    public class BuildingPieceFactory
    {
        private readonly BuildingPieceSpawner.Factory _pieceFactory;

        public BuildingPieceFactory(BuildingPieceSpawner.Factory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public BuildingPieceSpawner CreatePiece(PieceType pieceType, Position position, BuildState buildState)
        {
            var piece = _pieceFactory.Create(new PieceRenderInfo(pieceType), position, buildState);
            return piece;
        }
    }
}