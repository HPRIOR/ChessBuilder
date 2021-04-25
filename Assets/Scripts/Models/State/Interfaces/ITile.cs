using System;

namespace Models.State.Interfaces
{
    public interface ITile : ICloneable
    {
        IBoardPosition BoardPosition { get; set; }
        Piece.Piece CurrentPiece { get; set; }
    }
}