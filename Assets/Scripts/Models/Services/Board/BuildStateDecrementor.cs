using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Board
{
    public class BuildStateDecrementor
    {
        public static IEnumerable<Position> DecrementBuilds(BoardState boardState)
        {
            var decrementedTiles = new List<Position>();
            foreach (var tile in boardState.Board)
            {
                var pieceBeingBuilt = tile.BuildTileState.BuildingPiece != PieceType.NullPiece;
                if (pieceBeingBuilt)
                {
                    tile.BuildTileState = tile.BuildTileState.Decrement();
                    decrementedTiles.Add(tile.Position);
                }
            }

            // TODO: Problem! this will return tiles which were not decremented but remain at 0 
            return decrementedTiles;
        }
    }
}