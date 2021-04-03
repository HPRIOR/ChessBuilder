using System.Collections.Generic;
using UnityEngine;

public class NullPossibleMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState) =>
        new List<IBoardPosition>();
}
