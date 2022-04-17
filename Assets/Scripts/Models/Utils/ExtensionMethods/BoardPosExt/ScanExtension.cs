using System;
using System.Collections.Generic;
using Models.Services.Moves.Utils;
using Models.Services.Utils;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPosExt
{
    public static class ScanExtension
    {
        private static IEnumerable<Position> BaseScan(Position position, Direction direction,
            Predicate<Position> stopScanningPredicate, Action<List<Position>> startFunc = null)
        {
            var result = new List<Position>();
            startFunc?.Invoke(result);

            var iteratingPosition = position;

            while (true)
            {
                var newPosition = iteratingPosition.Add(Move.In(direction));

                if (stopScanningPredicate(newPosition)) break;

                result.Add(newPosition);
                iteratingPosition = newPosition;
            }

            return result;
        }

        public static IEnumerable<Position> Scan(this Position position, Direction direction) =>
            BaseScan(position, direction, PieceCannotMoveTo);

        /// <summary>
        ///     Returns positions between two points, excluding the first and second arguments
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static IEnumerable<Position> ScanBetween(this Position start, Position destination)
        {
            var direction = DirectionCache.DirectionFrom(start, destination);

            bool StopScanningPredicate(Position position) =>
                PieceCannotMoveTo(position) || position == destination;

            return BaseScan(start, direction, StopScanningPredicate);
        }


        /// <summary>
        ///     Returns positions between two points, excluding the first argument, including the second
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static IEnumerable<Position> ScanTo(this Position start, Position destination)
        {
            var direction = DirectionCache.DirectionFrom(start, destination);

            bool StopScanningPredicate(Position position) =>
                PieceCannotMoveTo(position) || position == destination.Add(Move.In(direction));

            return BaseScan(start, direction, StopScanningPredicate);
        }

        /// <summary>
        ///     Returns positions between two points, including first and second arguments
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static IEnumerable<Position> ScanInclusiveTo(this Position start, Position destination)
        {
            var direction = DirectionCache.DirectionFrom(start, destination);

            bool StopScanningPredicate(Position position) =>
                PieceCannotMoveTo(position) || position == destination.Add(Move.In(direction));

            void StartScanningAction(List<Position> positions) =>
                positions.Add(start);

            return BaseScan(start, direction, StopScanningPredicate, StartScanningAction);
        }


        private static bool PieceCannotMoveTo(Position position)
        {
            var x = position.X;
            var y = position.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }
    }
}