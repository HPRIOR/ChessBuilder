using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;

namespace Controllers.PieceMovers
{
    public class MoveValidator : IMoveValidator
    {
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