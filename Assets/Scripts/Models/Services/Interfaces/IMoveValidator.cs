using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IMoveValidator
    {
        bool ValidateMove(IDictionary<Position, HashSet<Position>> possibleMoves, Position origin,
            Position destination);
    }
}