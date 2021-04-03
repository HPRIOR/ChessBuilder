using System.Collections.Generic;

public class MoveValidator : IMoveValidator
{
    public bool ValidateMove(IDictionary<IBoardPosition, HashSet<IBoardPosition>> possibleMoves, IBoardPosition from, IBoardPosition destination)
    {
        if (from == destination) return false;
        if (possibleMoves.ContainsKey(from))
            return possibleMoves[from].Contains(destination);
        return false;
    }
}