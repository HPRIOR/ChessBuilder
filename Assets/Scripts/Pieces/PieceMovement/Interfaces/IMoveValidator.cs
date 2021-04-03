using System.Collections.Generic;

public interface IMoveValidator
{
    bool ValidateMove(IDictionary<IBoardPosition, HashSet<IBoardPosition>> possibleMoves, IBoardPosition origin, IBoardPosition destination);
}