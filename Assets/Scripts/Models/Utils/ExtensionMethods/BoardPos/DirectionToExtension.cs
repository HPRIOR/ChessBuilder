using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class DirectionToExtension
    {
        public static Direction DirectionTo(this Position origin, Position target) =>
            origin switch
            {
                var xPos when target.X == xPos.X => origin switch
                {
                    var yPos when target.Y > yPos.Y => Direction.N,
                    var yPos when target.Y < yPos.Y => Direction.S,
                    _ => throw new DirectionException("No direction found")
                },
                var xPos when target.X > xPos.X => origin switch
                {
                    var yPos when target.Y == yPos.Y => Direction.E,
                    var yPos when target.Y > yPos.Y => Direction.NE,
                    var yPos when target.Y < yPos.Y => Direction.SE,
                    _ => throw new DirectionException("No direction found")
                },
                var xPos when target.X < xPos.X => origin switch
                {
                    var yPos when target.Y == yPos.Y => Direction.W,
                    var yPos when target.Y > yPos.Y => Direction.NW,
                    var yPos when target.Y < yPos.Y => Direction.SW,
                    _ => throw new DirectionException("No direction found")
                },
                _ => throw new DirectionException("No direction found")
            };
    }
}