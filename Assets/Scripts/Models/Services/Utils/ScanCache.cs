using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.Utils.ExtensionMethods.BoardPosExt;

namespace Models.Services.Utils
{
    public class ScanCache
    {
        private static readonly Dictionary<PositionDirection, IEnumerable<Position>> ScanPositions;
        private static readonly Dictionary<PositionPair, IEnumerable<Position>> ScanToPositions;
        private static readonly Dictionary<PositionPair, IEnumerable<Position>> ScanBetweenPositions;
        private static readonly Dictionary<PositionPair, IEnumerable<Position>> ScanInclusiveToPositions;

        static ScanCache()
        {
            ScanPositions = GetScanPositions();
            ScanBetweenPositions = GetScanBetweenPositions();
            ScanToPositions = GetScanToPositions();
            ScanInclusiveToPositions = GetScanInclusiveToPositions();
        }



        /// <summary>
        /// Returns a list of positions to the end of the board, excluding the first argument.
        /// </summary>
        /// <remarks>
        /// Results are cached rather than computed on method call.
        /// </remarks>
        /// <param name="p"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static IEnumerable<Position> Scan(Position p, Direction d)
        {
            var positionDirection = new PositionDirection(p, d);
            return ScanPositions.ContainsKey(positionDirection)
                ? ScanPositions[positionDirection]
                : new List<Position>();
        }

        /// <summary>
        /// Returns list of positions between arguments, excluding the first argument, including second argument.
        /// </summary>
        /// <remarks>
        /// Results are cached rather than computed on method call.
        /// </remarks>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static IEnumerable<Position> ScanTo(Position p1, Position p2)
        {
            var positionPair = new PositionPair(p1, p2);
            return ScanToPositions.ContainsKey(positionPair) ? ScanToPositions[positionPair] : new List<Position>();
        }

        /// <summary>
        /// Returns all positions between two points, excluding the input arguments.
        /// </summary>
        /// <remarks>
        /// Results are cached rather than computed on method call.
        /// </remarks>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static IEnumerable<Position> ScanBetween(Position p1, Position p2)
        {
            var positionPair = new PositionPair(p1, p2);
            return ScanBetweenPositions.ContainsKey(positionPair)
                ? ScanBetweenPositions[positionPair]
                : new List<Position>();
        }


        public static IEnumerable<Position> ScanInclusiveTo(Position p1, Position p2)
        {
            var positionPair = new PositionPair(p1, p2);
            return ScanInclusiveToPositions.ContainsKey(positionPair)
                ? ScanInclusiveToPositions[positionPair]
                : new List<Position>();
        }
        
        private static Dictionary<PositionDirection, IEnumerable<Position>> GetScanPositions()
        {
            var result = new Dictionary<PositionDirection, IEnumerable<Position>>(new PositionDirectionComparer());
            var positions = GetPositions();
            foreach (var position1 in positions)
            foreach (var position2 in positions)
                if (position1 != position2)
                {
                    var direction = DirectionCache.DirectionFrom(position1, position2);
                    if (direction != Direction.Null)
                        result[new PositionDirection(position1, direction)] = position1.Scan(direction);
                }

            return result;
        }


        private static Dictionary<PositionPair, IEnumerable<Position>> GetScanToPositions()
        {
            var result = new Dictionary<PositionPair, IEnumerable<Position>>(new PositionPairComparer());
            var positions = GetPositions();
            foreach (var position1 in positions)
            foreach (var position2 in positions)
                if (position1 != position2)
                {
                    var direction = DirectionCache.DirectionFrom(position1, position2);
                    if (direction != Direction.Null) 
                        result[new PositionPair(position1, position2)] = position1.ScanTo(position2);
                }

            return result;
        }

        private static Dictionary<PositionPair, IEnumerable<Position>> GetScanBetweenPositions()
        {
            var result = new Dictionary<PositionPair, IEnumerable<Position>>(new PositionPairComparer());
            var positions = GetPositions();
            foreach (var position1 in positions)
            foreach (var position2 in positions)
                if (position1 != position2)
                {
                    var direction = DirectionCache.DirectionFrom(position1, position2);
                    if (direction != Direction.Null) 
                        result[new PositionPair(position1, position2)] = position1.ScanBetween(position2);
                }

            return result;
        }

        private static Dictionary<PositionPair,IEnumerable<Position>> GetScanInclusiveToPositions()
        {
            var result = new Dictionary<PositionPair, IEnumerable<Position>>(new PositionPairComparer());
            var positions = GetPositions();
            foreach(var position1 in positions)
            foreach(var position2 in positions)
                if (position1 != position2)
                {
                    var direction = DirectionCache.DirectionFrom(position1, position2);
                    if (direction != Direction.Null)
                        result[new PositionPair(position1, position2)] = position1.ScanInclusiveTo(position2);
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