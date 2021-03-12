using UnityEngine;

public class Tile : ITile
{
    public Piece CurrentPiece { get; set; }
    public IBoardPosition BoardPosition { get; set; }

    public Tile(BoardPosition boardPosition, Piece currentPiece)
    {
        BoardPosition = boardPosition;
        CurrentPiece = currentPiece;
    }

    public Tile(BoardPosition boardPosition)
    {
        BoardPosition = boardPosition;
        CurrentPiece = null;
    }
}