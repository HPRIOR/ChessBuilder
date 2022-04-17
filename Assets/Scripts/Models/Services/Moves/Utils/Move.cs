using System;
using Models.State.Board;

namespace Models.Services.Moves.Utils
{
    public static class Move
    {
        public static Position In(Direction direction) => direction switch
        {
            Direction.N => new Position(0, 1),
            Direction.Ne => new Position(1, 1),
            Direction.E => new Position(1, 0),
            Direction.Se => new Position(1, -1),
            Direction.S => new Position(0, -1),
            Direction.Sw => new Position(-1, -1),
            Direction.W => new Position(-1, 0),
            Direction.Nw => new Position(-1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "You what? North Where?")
        };
    }
}