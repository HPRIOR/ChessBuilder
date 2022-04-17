using System.Collections.Generic;
using Models.State.Board;

namespace Controllers.Interfaces
{
    public interface IMoveValidator
    {
        bool ValidateMove(IDictionary<Position, List<Position>> possibleMoves, Position origin,
            Position destination);
    }
}