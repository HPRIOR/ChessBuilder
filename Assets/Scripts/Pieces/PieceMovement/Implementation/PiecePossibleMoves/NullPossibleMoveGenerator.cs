using System.Collections.Generic;
using UnityEngine;

public class NullPossibleMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece) => new List<IBoardPosition>();
}