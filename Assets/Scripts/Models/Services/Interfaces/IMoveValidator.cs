using System.Collections.Generic;
using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IMoveValidator
    {
        bool ValidateMove(IDictionary<IBoardPosition, HashSet<IBoardPosition>> possibleMoves, IBoardPosition origin, IBoardPosition destination);
    }
}