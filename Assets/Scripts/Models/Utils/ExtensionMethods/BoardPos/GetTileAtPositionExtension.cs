using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class GetTileAtPositionExtension
    {
        public static Tile TileIn(this Position position, BoardState boardState) =>
            boardState.Board[position.X, position.Y];
    }
}