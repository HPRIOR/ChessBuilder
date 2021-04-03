using System.Collections.Generic;
using UnityEngine;

public interface IPossibleBoardMovesGenerator
{
    IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState);
}