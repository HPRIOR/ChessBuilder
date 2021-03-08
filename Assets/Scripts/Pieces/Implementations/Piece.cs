using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour, IPiece
{
    public ITile Tile { get; set; }

    private void Update()
    {
        if (Tile.CurrentPiece == PieceType.None)
            Destroy(gameObject);
    }
}
