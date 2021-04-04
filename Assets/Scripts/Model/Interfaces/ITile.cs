using System;

public interface ITile : ICloneable
{
    IBoardPosition BoardPosition { get; set; }
    PieceType CurrentPiece { get; set; }
}