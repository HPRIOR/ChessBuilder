using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;

namespace Controllers.PieceMovers
{
    public class MoveValidator : IMoveValidator
    {
        /*
         * Keep move validator taking possible move argument. This will allow it to be used in other commands, and on
         * arbitrary board states
         */
        public bool ValidateMove(IDictionary<BoardPosition, HashSet<BoardPosition>> possibleMoves,
            BoardPosition from, BoardPosition destination)
        {
            if (from.Equals(destination)) return false;
            if (possibleMoves.ContainsKey(from))
                return possibleMoves[from].Contains(destination);
            return false;
        }
    }
}