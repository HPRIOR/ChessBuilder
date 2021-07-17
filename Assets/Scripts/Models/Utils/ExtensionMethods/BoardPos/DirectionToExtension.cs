using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class DirectionToExtension
    {
        public static Direction DirectionTo(this Position origin, Position target)
        {
            return origin switch
            {
                var pos when target.X == pos.X && target.Y > pos.Y => Direction.N,
                var pos when target.X == pos.X && target.Y < pos.Y => Direction.S,
                var pos when target.X > pos.X && target.Y == pos.Y => Direction.E,
                var pos when target.X < pos.X && target.Y == pos.Y => Direction.W,
                var pos when target.X > pos.X && target.Y > pos.Y => Direction.NE,
                var pos when target.X < pos.X && target.Y > pos.Y => Direction.NW,
                var pos when target.X > pos.X && target.Y < pos.Y => Direction.SE,
                var pos when target.X < pos.X && target.Y < pos.Y => Direction.SW,
                _ => throw new DirectionException("No direction found")
            };
        }
    }
}