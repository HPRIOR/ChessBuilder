using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour, IPiece
{
    public IBoardPosition boardPosition { get; set; }

    public PieceColour pieceColour { get; set; }

    public PieceType pieceType { get; set; }

}
