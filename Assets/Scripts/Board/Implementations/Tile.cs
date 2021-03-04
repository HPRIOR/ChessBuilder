using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ITile
{
    public PieceType CurrentPiece { get; set; }
    public IBoardPosition BoardPosition { get; set; }

    public Tile(BoardPosition boardPosition, PieceType currentPiece)
    {
        BoardPosition = boardPosition;
        CurrentPiece = currentPiece;
    }
}
