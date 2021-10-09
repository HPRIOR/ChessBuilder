using System;
using Models.Services.Moves.Utils;
using Models.State.Board;
using UnityEngine.Animations;

namespace Models.Utils.ExtensionMethods.BoardPosExt
{
    public static class DirectionToExtension
    {
        public static Direction DirectionTo(this Position origin, Position target)
        {
            if (target == origin)
                throw new DirectionException("Target cannot be the same as origin");
            if (target.X == origin.X) return HandleNorthSouthDirections(origin, target);
            if (target.Y == origin.Y) return HandleEastWestDirections(origin, target);
            return HandleDiagonals(origin, target);
        }

        private static Direction HandleDiagonals(Position origin, Position target)
        {
            var minusX = target.X - origin.X;
            var minusY = target.Y - origin.Y;
            if (Math.Abs(minusX) == Math.Abs(minusY))
            {
                if (minusX > 0 & minusY > 0) return Direction.NE;
                if (minusX > 0 & minusY < 0) return Direction.SE;
                if (minusX < 0 & minusY < 0) return Direction.SW;
                if (minusX < 0 & minusY > 0) return Direction.NW;
            }

            return Direction.Null;
        }

        private static Direction HandleEastWestDirections(Position origin, Position target)
        {
            if (target.X > origin.X & target.Y == origin.Y)
                return Direction.E;
            if (target.X < origin.X & target.Y == origin.Y)
                return Direction.W;
            return Direction.Null;
        }

        private static Direction HandleNorthSouthDirections(Position origin, Position target)
        {
            if (target.Y > origin.Y & target.X == origin.X)
                return Direction.N;
            if (target.Y < origin.Y & target.X == origin.X)
                return Direction.S;
            return Direction.Null;
        }
    }
}