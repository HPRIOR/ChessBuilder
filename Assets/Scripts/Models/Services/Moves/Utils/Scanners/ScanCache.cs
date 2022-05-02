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
            Direction.Ne, Direction.Nw, Direction.Se, Direction.Sw
        };

        private static readonly Dictionary<PositionDirection, Position[]> Cache = GetScanPositions();

        public static Position[] GetPositionsToEndOfBoard(Position pos, Direction direction) =>
            Cache[new PositionDirection(pos, direction)];

        private static Dictionary<PositionDirection, Position[]> GetScanPositions()
        {
            var result = new Dictionary<PositionDirection, Position[]>(new PositionDirectionComparer());
            var positions = GetPositions();
            for (var index0 = 0; index0 < positions.GetLength(0); index0++)
            for (var index1 = 0; index1 < positions.GetLength(1); index1++)
            {
                var position = positions[index0, index1];
                for (var index = 0; index < Directions.Length; index++)
                {
                    var direction = Directions[index];
                    result[new PositionDirection(position, direction)] = position.Scan(direction).ToArray();
                }
            }

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