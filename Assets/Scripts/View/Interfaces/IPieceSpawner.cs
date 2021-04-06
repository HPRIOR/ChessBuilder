public interface IPieceSpawner
{
    PieceMono CreatePiece(PieceType pieceType, IBoardPosition BoardPosition);
}