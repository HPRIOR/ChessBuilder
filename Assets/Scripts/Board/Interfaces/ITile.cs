using UnityEngine;

public interface ITile
{
    IBoardPosition BoardPosition { get; set; }
    GameObject CurrentPiece { get; set; }
}