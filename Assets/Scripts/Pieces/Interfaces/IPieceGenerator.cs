using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPieceGenerator
{
    void GeneratePiece(ITile tile, PieceType pieceType);
}
