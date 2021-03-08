using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour, IPiece
{
    public ITile PieceTile { get; set; }

    private void Update()
    {
        if (PieceTile.CurrentPiece == PieceType.None)
            Destroy(gameObject);
    }
}
