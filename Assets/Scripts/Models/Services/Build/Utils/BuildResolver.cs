using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;

namespace Models.Services.Build.Utils
{
    public class BuildResolver : IBuildResolver
    {
        // this could be moved to the GameController so that it's calling would not be repeated
        // however, game logic would require the game controller to progress 
        public void ResolveBuilds(BoardState boardState, PieceColour turn)
        {
            foreach (var tile in boardState.Board)
            {
                var canBuild = tile.BuildTileState.Turns == 0 &&
                               tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                               tile.CurrentPiece.Type == PieceType.NullPiece &&
                               tile.BuildTileState.BuildingPiece.Colour() == turn;
                if (canBuild)
                {
                    tile.CurrentPiece = new Piece(tile.BuildTileState.BuildingPiece);
                    tile.BuildTileState = new BuildTileState(); // reset build state
                }
            }
        }
    }
}