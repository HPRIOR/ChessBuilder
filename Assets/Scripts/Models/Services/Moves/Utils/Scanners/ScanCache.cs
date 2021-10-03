using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Utils;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPosExt;

namespace Models.Services.Moves.Utils.Scanners
{
    public static class ScanCache
    {
        private static readonly Direction[] Directions =
        {
            Direction.N, Direction.E, Direction.S, Direction.W,
            Direction.NE, Direction.NW, Direction.SE, Direction.SW
        };

        private static readonly Dictionary<PositionDirection, Position[]> Cache = GetScanPositions();

        public static Span<Position> GetPositionsToEndOfBoard(Position pos, Direction direction) =>
            Cache[new PositionDirection(pos, direction)].AsSpan();

        private static Dictionary<PositionDirection, Position[]> GetScanPositions()
        {
            var result = new Dictionary<PositionDirection, Position[]>(new PositionDirectionComparer());
            var positions = GetPositions();
            foreach (var position in positions)
            foreach (var direction in Directions)
                result[new PositionDirection(position, direction)] = position.Scan(direction).ToArray();

            return result;
        }

        private static Position[,] GetPositions()
        {
            var positions = new Position[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                positions[i, j] = new Position(i, j);
            return positions;
        }
    }
}