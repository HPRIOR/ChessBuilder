using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPosExt;

namespace Models.Services.Utils
{
    public static class ScanCache
    {
        private static readonly Dictionary<PositionDirection, Position[]> ScanPositions;
        private static readonly Dictionary<PositionPair, Position[]> ScanToPositions;
        private static readonly Dictionary<PositionPair, Position[]> ScanBetweenPositions;
        private static readonly Dictionary<PositionPair, Position[]> ScanInclusiveToPositions;

        private static readonly Direction[] Directions =
        {
            Direction.N, Direction.E, Direction.S, Direction.W,
            Direction.Ne, Direction.Nw, Direction.Se, Direction.Sw
        };

        static ScanCache()
        {
            ScanPositions = GetScanPositions();
            ScanBetweenPositions = GetScanBetweenPositions();
            ScanToPositions = GetScanToPositions();
            ScanInclusiveToPositions = GetScanInclusiveToPositions();
        }


        /// <summary>
        ///     Returns an array of positions to the end of the board, excluding the first argument.
        /// </summary>
        /// <remarks>
        ///     Results are cached rather than computed on method call.
        /// </remarks>
        /// <param name="p"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Position[] Scan(Position p, Direction d)
        {
            var positionDirection = new PositionDirection(p, d);
            return ScanPositions.ContainsKey(positionDirection)
                ? ScanPositions[positionDirection]
                : Array.Empty<Position>();
        }

        /// <summary>
        ///     Returns array of positions between arguments, excluding the first argument, including second argument.
        /// </summary>
        /// <remarks>
        ///     Results are cached rather than computed on method call.
        /// </remarks>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Position[] ScanTo(Position p1, Position p2)
        {
            var positionPair = new PositionPair(p1, p2);
            return ScanToPositions.ContainsKey(positionPair) ? ScanToPositions[positionPair] : Array.Empty<Position>();
        }

        /// <summary>
        ///     Returns all positions between two points, excluding the input arguments.
        /// </summary>
        /// <remarks>
        ///     Results are cached rather than computed on method call.
        /// </remarks>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Position[] ScanBetween(Position p1, Position p2)
        {
            var positionPair = new PositionPair(p1, p2);
            return ScanBetweenPositions.ContainsKey(positionPair)
                ? ScanBetweenPositions[positionPair]
                : Array.Empty<Position>();
        }


        public static Position[] ScanInclusiveTo(Position p1, Position p2)
        {
            var positionPair = new PositionPair(p1, p2);
            return ScanInclusiveToPositions.ContainsKey(positionPair)
                ? ScanInclusiveToPositions[positionPair]
                : Array.Empty<Position>();
        }

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


        private static Dictionary<PositionPair, Position[]> GetScanToPositions()
        {
            var result = new Dictionary<PositionPair, Position[]>(new PositionPairComparer());
            var positions = GetPositions();
            for (var index0 = 0; index0 < positions.GetLength(0); index0++)
            for (var index1 = 0; index1 < positions.GetLength(1); index1++)
            {
                var position1 = positions[index0, index1];
                foreach (var position2 in positions)
                    if (position1 != position2)
                    {
                        var direction = DirectionCache.DirectionFrom(position1, position2);
                        if (direction != Direction.Null)
                            result[new PositionPair(position1, position2)] = position1.ScanTo(position2).ToArray();
                    }
            }

            return result;
        }

        private static Dictionary<PositionPair, Position[]> GetScanBetweenPositions()
        {
            var result = new Dictionary<PositionPair, Position[]>(new PositionPairComparer());
            var positions = GetPositions();
            for (var index0 = 0; index0 < positions.GetLength(0); index0++)
            for (var index1 = 0; index1 < positions.GetLength(1); index1++)
            {
                var position1 = positions[index0, index1];
                foreach (var position2 in positions)
                    if (position1 != position2)
                    {
                        var direction = DirectionCache.DirectionFrom(position1, position2);
                        if (direction != Direction.Null)
                            result[new PositionPair(position1, position2)] = position1.ScanBetween(position2).ToArray();
                    }
            }

            return result;
        }

        private static Dictionary<PositionPair, Position[]> GetScanInclusiveToPositions()
        {
            var result = new Dictionary<PositionPair, Position[]>(new PositionPairComparer());
            var positions = GetPositions();
            for (var index0 = 0; index0 < positions.GetLength(0); index0++)
            for (var index1 = 0; index1 < positions.GetLength(1); index1++)
            {
                var position1 = positions[index0, index1];
                foreach (var position2 in positions)
                    if (position1 != position2)
                    {
                        var direction = DirectionCache.DirectionFrom(position1, position2);
                        if (direction != Direction.Null)
                            result[new PositionPair(position1, position2)] =
                                position1.ScanInclusiveTo(position2).ToArray();
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