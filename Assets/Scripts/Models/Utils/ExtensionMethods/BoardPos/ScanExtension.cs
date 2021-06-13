using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class ScanExtension
    {
        public static IEnumerable<Position> Scan(this Position position, Direction inDirection)
        {
            var newPosition = position.Add(Move.In(inDirection));
            return PieceCannotMoveTo(newPosition)
                ? new List<Position>()
                : Scan(newPosition, inDirection).Concat(new List<Position> {newPosition});
        }

        private static bool PieceCannotMoveTo(Position position)
        {
            var x = position.X;
            var y = position.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }
    }
}