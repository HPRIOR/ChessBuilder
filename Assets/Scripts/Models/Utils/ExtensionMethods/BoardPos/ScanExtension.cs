using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;

namespace Models.Utils.ExtensionMethods.BoardPos
{
    public static class ScanExtension
    {
        public static IEnumerable<BoardPosition> Scan(this BoardPosition position, Direction inDirection)
        {
            var newPosition = position.Add(Move.In(inDirection));
            return PieceCannotMoveTo(newPosition)
                ? new List<BoardPosition>()
                : Scan(newPosition, inDirection).Concat(new List<BoardPosition> {newPosition});
        }

        private static bool PieceCannotMoveTo(BoardPosition boardPosition)
        {
            var x = boardPosition.X;
            var y = boardPosition.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }
    }
}