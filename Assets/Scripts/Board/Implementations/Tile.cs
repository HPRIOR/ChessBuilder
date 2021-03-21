using UnityEngine;

public class Tile : ITile
{
    public GameObject CurrentPiece { get; set; }
    public IBoardPosition BoardPosition { get; set; }

    public Tile(BoardPosition boardPosition, GameObject currentPiece)
    {
        BoardPosition = boardPosition;
        CurrentPiece = currentPiece;
    }

    public Tile(BoardPosition boardPosition)
    {
        BoardPosition = boardPosition;
        CurrentPiece = null;
    }

    public override string ToString() => $"Tile at ({BoardPosition.X}, {BoardPosition.Y}) containing" +
        $" {CurrentPiece?.GetComponent<Piece>().Info.PieceType}";
}