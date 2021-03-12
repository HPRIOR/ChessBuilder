using UnityEngine;

public interface ITile
{
    IBoardPosition BoardPosition { get; set; }
    Piece CurrentPiece { get; set; }
}