﻿using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class DirectionToExtension
    {
        public static Direction DirectionTo(this Position origin, Position target)
        {
            if (target.X == origin.X && target.Y > origin.Y) return Direction.N;
            if (target.X == origin.X && target.Y < origin.Y) return Direction.S;
            if (target.X > origin.X && target.Y == origin.Y) return Direction.E;
            if (target.X < origin.X && target.Y == origin.Y) return Direction.W;
            if (target.X > origin.X && target.Y > origin.Y) return Direction.NE;
            if (target.X < origin.X && target.Y > origin.Y) return Direction.NW;
            if (target.X > origin.X && target.Y < origin.Y) return Direction.SE;
            if (target.X < origin.X && target.Y < origin.Y) return Direction.SW;
            throw new DirectionException("No direction found");
        }
    }
}