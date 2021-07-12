using System.Collections.Immutable;
using Models.State.Board;

namespace Controllers.Interfaces
{
    public interface IMoveValidator
    {
        bool ValidateMove(ImmutableDictionary<Position, ImmutableHashSet<Position>> possibleMoves, Position origin,
            Position destination);
    }
}