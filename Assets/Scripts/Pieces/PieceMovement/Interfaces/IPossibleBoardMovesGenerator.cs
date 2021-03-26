using System.Collections.Generic;
using UnityEngine;

public interface IPossibleBoardMovesGenerator
{
    IDictionary<GameObject, HashSet<IBoardPosition>> PossibleMoves { get; }

    void GeneratePossibleMoves();
}