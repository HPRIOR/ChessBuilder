using Zenject;

public class BoardEval : IBoardEval
{

    /*
     * Changes: account for removal of GO from board.
     * Tile current piece will be piece type - add mechanism for getting piece colour from piece type
     * and use this to compare with current piece colour
     */
    private readonly PieceColour _pieceColour;

    public BoardEval(PieceColour pieceColour)
    {
        _pieceColour = pieceColour;
    }

    public bool NoPieceIn(ITile tile) => tile.CurrentPiece == null;

    public bool FriendlyPieceIn(ITile tile) =>
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour == _pieceColour;

    public bool OpposingPieceIn(ITile tile) =>
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour != _pieceColour;

    public class Factory : PlaceholderFactory<PieceColour, BoardEval> { }
}