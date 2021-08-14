using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Board
{
    public class BuildStateDecrementor
    {
        public void DecrementBuilds(BoardState boardState)
        {
            foreach (var tile in boardState.Board)
            {
                var pieceBeingBuilt = tile.BuildTileState.BuildingPiece != PieceType.NullPiece;
                if (pieceBeingBuilt)
                    tile.BuildTileState = tile.BuildTileState.Decrement();
            }
        }
    }
}