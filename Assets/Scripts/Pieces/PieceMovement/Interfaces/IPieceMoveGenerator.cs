using System.Collections.Generic;
using UnityEngine;

public interface IPieceMoveGenerator
{
    IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState);
}