using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile 
{
    IBoardPosition BoardPosition { get; set; }
    PieceType CurrentPiece { get; set; }

}
