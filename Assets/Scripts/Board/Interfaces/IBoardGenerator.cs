using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardGenerator 
{
    Tile[,] GenerateBoard();
}
