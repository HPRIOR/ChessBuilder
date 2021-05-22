using System;

namespace Models.State.Interfaces
{
    public interface ITile : ICloneable
    {
        IBoardPosition BoardPosition { get; set; }
        PieceState.Piece CurrentPiece { get; set; }
    }
}