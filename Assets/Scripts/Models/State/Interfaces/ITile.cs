using System;
using Models.State.PieceState;

namespace Models.State.Interfaces
{
    public interface ITile : ICloneable
    {
        IBoardPosition BoardPosition { get; }
        Piece CurrentPiece { get; set; }
    }
}