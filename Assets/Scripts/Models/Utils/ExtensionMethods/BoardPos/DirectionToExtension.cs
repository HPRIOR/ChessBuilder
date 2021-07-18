using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class DirectionToExtension
    {
        public static Direction DirectionTo(this Position origin, Position target)
        {
            if (target.X == origin.X)
                return target.Y > origin.Y ? Direction.N :
                    target.Y == origin.Y ? throw new DirectionException("No direction found") : Direction.S;
            if (target.X > origin.X)
                return target.Y == origin.Y ? Direction.E : target.Y > origin.Y ? Direction.NE : Direction.SE;
            if (target.X < origin.X)
                return target.Y == origin.Y ? Direction.W : target.Y > origin.Y ? Direction.NW : Direction.SW;

            throw new DirectionException("No direction found");
        }
    }
}