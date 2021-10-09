using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Board
{
    public sealed class BuildStateDecrementor
    {
        public static IEnumerable<Position> DecrementBuilds(BoardState boardState)
        {
            var decrementedTiles = new List<Position>();
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                ref var tile = ref boardState.GetTileAt(i,j);
                var pieceBeingBuilt = tile.BuildTileState.BuildingPiece != PieceType.NullPiece;
                if (pieceBeingBuilt)
                {
                    var previousState = tile.BuildTileState;
                    tile.BuildTileState = tile.BuildTileState.Decrement();
                    if (previousState.Turns != tile.BuildTileState.Turns)
                        // ensure that tiles which are not decremented will not be incremented when reversed
                        decrementedTiles.Add(tile.Position);
                }
            }

            return decrementedTiles;
        }
    }
}