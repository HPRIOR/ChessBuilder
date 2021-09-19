using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Moves.Utils.Scanners
{
    public static class ScanCache
    {
        private static readonly Direction[] Directions =
        {
            Direction.N, Direction.E, Direction.S, Direction.W,
            Direction.NE, Direction.NW, Direction.SE, Direction.SW
        };

        private static readonly Dictionary<Position, Dictionary<Direction, Position[]>> Cache = GetCache();
        public static Position[] GetPositionsToEndOfBoard(Position pos, Direction direction) => Cache[pos][direction];

        private static Dictionary<Position, Dictionary<Direction, Position[]>> GetCache()
        {
            var result = new Dictionary<Position, Dictionary<Direction, Position[]>>();
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                var position = new Position(i, j);
                result.Add(position, GetPositionsToEndOfBoard(position));
            }

            return result;
        }

        private static Dictionary<Direction, Position[]> GetPositionsToEndOfBoard(Position pos)
        {
            var result = new Dictionary<Direction, Position[]>(new DirectionComparer());
            foreach (var direction in Directions) result.Add(direction, GetPositionToEndOfBoard(pos, direction));

            return result;
        }

        private static Position[] GetPositionToEndOfBoard(Position pos, Direction dir)
        {
            var result = new List<Position>();
            var iteratingPosition = pos.Add(Move.In(dir));
            while (!PositionOutOfBounds(iteratingPosition))
            {
                result.Add(iteratingPosition);
                iteratingPosition = iteratingPosition.Add(Move.In(dir));
            }

            return result.ToArray();
        }

        private static bool PositionOutOfBounds(Position pos)
        {
            var x = pos.X;
            var y = pos.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }
    }
}