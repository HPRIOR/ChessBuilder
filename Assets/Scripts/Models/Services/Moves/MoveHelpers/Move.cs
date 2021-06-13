using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Moves.MoveHelpers
{
    public static class Move
    {
        private static readonly IDictionary<Direction, Position> DirectionMovement =
            new Dictionary<Direction, Position>
            {
                {Direction.N, new Position(0, 1)},
                {Direction.E, new Position(1, 0)},
                {Direction.S, new Position(0, -1)},
                {Direction.W, new Position(-1, 0)},
                {Direction.NE, new Position(1, 1)},
                {Direction.SE, new Position(1, -1)},
                {Direction.SW, new Position(-1, -1)},
                {Direction.NW, new Position(-1, 1)}
            };

        public static Position In(Direction direction) => DirectionMovement[direction];
    }
}