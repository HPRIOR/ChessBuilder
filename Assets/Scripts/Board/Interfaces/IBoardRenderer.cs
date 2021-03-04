using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardRenderer
{
    void RenderBoard(ITile[,] board, GameObject tilePrefab);
}
