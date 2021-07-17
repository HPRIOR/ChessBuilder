using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class ScanExtension
    {
        public static IEnumerable<Position> Scan(this Position position, Direction direction)
        {
            var result = new List<Position>();
            var iteratingPosition = position;

            while (true)
            {
                var newPosition = iteratingPosition.Add(Move.In(direction));

                if (PieceCannotMoveTo(newPosition)) break;

                result.Add(newPosition);
                iteratingPosition = newPosition;
            }

            return result;
        }

        public static IEnumerable<Position> ScanBetween(this Position start, Position destination)
        {
            var direction = start.DirectionTo(destination);

            bool StopScanningPredicate(Position position) =>
                PieceCannotMoveTo(position) || position.Equals(destination);

            return RecurseScan(start, direction, StopScanningPredicate);
        }


        public static IEnumerable<Position> ScanTo(this Position start, Position destination)
        {
            var direction = start.DirectionTo(destination);

            bool StopScanningPredicate(Position position) =>
                PieceCannotMoveTo(position) || position.Equals(destination.Add(Move.In(direction)));

            return RecurseScan(start, direction, StopScanningPredicate);
        }

        private static IEnumerable<Position> RecurseScan(Position position, Direction direction,
            Predicate<Position> stopScanningPredicate)
        {
            var newPosition = position.Add(Move.In(direction));
            return stopScanningPredicate(newPosition)
                ? new List<Position>()
                : new List<Position> {newPosition}.Concat(RecurseScan(newPosition, direction, stopScanningPredicate));
        }


        private static bool PieceCannotMoveTo(Position position)
        {
            var x = position.X;
            var y = position.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }
    }
}