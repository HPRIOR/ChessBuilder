using System.Collections.Immutable;
using Controllers.Interfaces;
using Models.State.Board;

namespace Controllers.Commands
{
    public class MoveValidator : IMoveValidator
    {
        /*
         * Keep move validator taking possible move argument. This will allow it to be used in other commands, and on
         * arbitrary board states
         */
        public bool ValidateMove(ImmutableDictionary<Position, ImmutableHashSet<Position>> possibleMoves,
            Position from, Position destination)
        {
            if (from == destination) return false;
            if (possibleMoves.ContainsKey(from))
                return possibleMoves[from].Contains(destination);
            return false;
        }
    }
}