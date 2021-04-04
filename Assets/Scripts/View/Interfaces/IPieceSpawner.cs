public interface IPieceSpawner
{
    Piece CreatePiece(PieceType pieceType, IBoardPosition BoardPosition);
}