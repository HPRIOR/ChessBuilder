using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPieceGanerator
{
    void GeneratePeice(ITile tile, PieceType pieceType);
}
