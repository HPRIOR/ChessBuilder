using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public static class ScanFrom
    {
        public static IEnumerable<BoardPosition> This(BoardPosition position, Direction inDirection)
        {
            var newPosition = position.Add(Move.In(inDirection));
            return PieceCannotMoveTo(newPosition)
                ? new List<BoardPosition>()
                : This(newPosition, inDirection).Concat(new List<BoardPosition> {newPosition});
        }

        private static bool PieceCannotMoveTo(BoardPosition boardPosition)
        {
            var x = boardPosition.X;
            var y = boardPosition.Y;
            return 0 > x || x > 7 || 0 > y || y > 7;
        }
    }
}