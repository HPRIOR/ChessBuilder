using System.Collections.Generic;
using UnityEngine;

public interface IPossibleMovesGenerator
{
    IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState);
}