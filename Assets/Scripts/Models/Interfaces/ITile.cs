using System;
using Assets.Scripts.Models.Piece;

public interface ITile : ICloneable
{
    IBoardPosition BoardPosition { get; set; }
    Piece CurrentPiece { get; set; }
}