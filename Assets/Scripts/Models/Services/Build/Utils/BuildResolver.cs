using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.Services.Build.Utils
{
    public class BuildResolver : IBuildResolver
    {
        public void ResolveBuild(BoardState boardState)
        {
            foreach (var tile in boardState.Board)
            {
                var buildIsZeroTileIsEmptyAndPieceIsBuilding = tile.BuildState.Turns == 0 &&
                                                               tile.BuildState.BuildingPiece.Type !=
                                                               PieceType.NullPiece &&
                                                               tile.CurrentPiece.Type == PieceType.NullPiece;
                if (buildIsZeroTileIsEmptyAndPieceIsBuilding)
                {
                    tile.CurrentPiece = tile.BuildState.BuildingPiece;
                    tile.BuildState = new BuildState(); // reset build state
                }
            }
        }
    }
}