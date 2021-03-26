using System.Collections.Generic;
using UnityEngine;

public interface IPieceMoveGenerator
{
    IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece);
}