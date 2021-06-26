using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using View.Interfaces;
using View.Utils;

namespace View.Renderers
{
    public class BuildRenderer : IBoardStateChangeRenderer
    {
        public void Render(BoardState previousState, BoardState newState)
        {
            GameObjectDestroyer.DestroyChildrenOfObjectWith("BuildingPieces");
            var board = newState.Board;
            foreach (var tile in board)
            {
                var isBuilding = tile.BuildState.BuildingPiece != PieceType.NullPiece;
                if (isBuilding) RenderBuild(tile.BuildState, tile.Position);
            }
        }

        private void RenderBuild(BuildState buildState, Position position)
        {
            // get piece info from piece type
            // use sprite from sprite path to render new sprite 
            // change sprite position tile position
        }
    }
}