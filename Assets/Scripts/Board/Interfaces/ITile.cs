using System;
using UnityEngine;

public interface ITile : ICloneable
{
    IBoardPosition BoardPosition { get; set; }
    PieceType CurrentPiece { get; set; }
}