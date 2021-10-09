using System;
using Models.State.Board;

namespace Models.Services.Moves.Utils
{
    public static class Move
    {
        public static Position In(Direction direction) => direction switch
        {
            Direction.N => new Position(0, 1),
            Direction.NE => new Position(1, 1),
            Direction.E => new Position(1, 0),
            Direction.SE => new Position(1, -1),
            Direction.S => new Position(0, -1),
            Direction.SW => new Position(-1, -1),
            Direction.W => new Position(-1, 0),
            Direction.NW => new Position(-1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "You what? North Where?")
        };
    }
}