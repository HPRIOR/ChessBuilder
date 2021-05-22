using System;

namespace Models.State.Interfaces
{
    public interface ITile : ICloneable
    {
        IBoardPosition BoardPosition { get; }
        PieceState.Piece CurrentPiece { get; set; }
    }
}