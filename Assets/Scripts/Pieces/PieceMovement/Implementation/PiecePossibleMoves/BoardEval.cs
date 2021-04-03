using Zenject;

public class BoardEval : IBoardEval
{

    private readonly PieceColour _pieceColour;

    public BoardEval(PieceColour pieceColour)
    {
        _pieceColour = pieceColour;
    }

    public bool NoPieceIn(ITile tile) => tile.CurrentPiece == PieceType.NullPiece;

    public bool FriendlyPieceIn(ITile tile) =>
        tile.CurrentPiece is PieceType.NullPiece ? false : PieceColourFromType(tile.CurrentPiece) == _pieceColour;

    public bool OpposingPieceIn(ITile tile) =>
        tile.CurrentPiece is PieceType.NullPiece ? false : PieceColourFromType(tile.CurrentPiece) != _pieceColour;

    private PieceColour PieceColourFromType(PieceType pieceType) => pieceType.ToString().StartsWith("White") ? PieceColour.White : PieceColour.Black;

    public class Factory : PlaceholderFactory<PieceColour, BoardEval> { }
}