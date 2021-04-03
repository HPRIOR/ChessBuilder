public class PieceSpawner : IPieceSpawner
{
    private readonly Piece.Factory _pieceFactory;

    public PieceSpawner(Piece.Factory pieceFactory)
    {
        _pieceFactory = pieceFactory;
    }

    public Piece CreatePiece(PieceType pieceType, IBoardPosition boardPosition)
    { 
        var piece = _pieceFactory.Create(new PieceInfo(pieceType), boardPosition);
        piece.BoardPosition = boardPosition;
        return piece;
    }

        
}