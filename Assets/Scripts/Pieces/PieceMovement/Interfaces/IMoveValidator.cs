using System.Collections.Generic;
using UnityEngine;

public interface IMoveValidator
{
    bool ValidateMove(IDictionary<IBoardPosition, HashSet<IBoardPosition>> possibleMoves, IBoardPosition origin, IBoardPosition destination);
}