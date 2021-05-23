using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IMoveValidator
    {
        bool ValidateMove(IDictionary<BoardPosition, HashSet<BoardPosition>> possibleMoves, BoardPosition origin,
            BoardPosition destination);
    }
}