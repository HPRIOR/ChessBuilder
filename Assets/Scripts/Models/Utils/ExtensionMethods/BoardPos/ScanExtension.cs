using System;
using System.Collections.Generic;
using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class ScanExtension
    {
        public static IEnumerable<Position> Scan(this Position position, Direction direction) =>
            BaseScan(position, direction, PieceCannotMoveTo);

        public static IEnumerable<Position> ScanBetween(this Position start, Position destination)
        {
            var direction = start.DirectionTo(destination);

            bool StopScanningPredicate(Position position) =>
                PieceCannotMoveTo(position) || position.Equals(destination);

            return BaseScan(start, direction, StopScanningPredicate);
        }


        public static IEnumerable<Position> ScanTo(this Position start, Position destination)
        {
            var direction = start.DirectionTo(destination);

            bool StopScanningPredicate(Position position) =>
                PieceCannotMoveTo(position) || position.Equals(destination.Add(Move.In(direction)));

            return BaseScan(start, direction, StopScanningPredicate);
        }

        private static IEnumerable<Position> BaseScan(Position position, Direction direction,
            Predicate<Position> stopScanningPredicate)
        {
            var result = new List<Position>();
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


        private static bool PieceCannotMoveTo(Position position)
        {
            var x = position.X;
            var y = position.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }
    }
}