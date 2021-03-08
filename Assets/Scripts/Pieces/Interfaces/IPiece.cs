using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPiece
{
    IBoardPosition boardPosition { get; set; }
    PieceColour pieceColour { get; set; }
    PieceType pieceType { get; set; }

}
