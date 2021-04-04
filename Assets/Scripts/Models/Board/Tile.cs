/*
 * Changes: CurrentPiece changed to piecType
 */

public class Tile : ITile
{
    public PieceType CurrentPiece { get; set; }
    public IBoardPosition BoardPosition { get; set; }

    public Tile(IBoardPosition boardPosition, PieceType currentPiece)
    {
        BoardPosition = boardPosition;
        CurrentPiece = currentPiece;
    }

    public Tile(BoardPosition boardPosition)
    {
        BoardPosition = boardPosition;
        CurrentPiece = PieceType.NullPiece;
    }

    public override string ToString() => $"Tile at ({BoardPosition.X}, {BoardPosition.Y}) containing" +
        $" {CurrentPiece}";

    public object Clone() =>
        new Tile(BoardPosition, CurrentPiece);
}