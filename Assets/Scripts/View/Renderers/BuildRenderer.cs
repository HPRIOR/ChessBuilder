using Models.State.Board;
using Models.State.PieceState;
using View.Interfaces;
using View.Utils;
using View.Utils.Prefab.Factories;

namespace View.Renderers
{
    public class BuildRenderer : IStateChangeRenderer
    {
        private readonly BuildingPieceFactory _buildingPieceFactory;

        public BuildRenderer(BuildingPieceFactory buildingPieceFactory)
        {
            _buildingPieceFactory = buildingPieceFactory;
        }

        public void Render(BoardState previousState, BoardState newState)
        {
            GameObjectDestroyer.DestroyChildrenOfObjectWith("BuildingPieces");
            var board = newState.Board;
            foreach (var tile in board)
            {
                var isBuilding = tile.BuildState.BuildingPiece != PieceType.NullPiece;
                if (isBuilding)
                    _buildingPieceFactory.Create(tile.BuildState, tile.Position);
            }
        }
    }
}