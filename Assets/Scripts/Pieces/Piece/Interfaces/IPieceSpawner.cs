public interface IPieceSpawner
{
    Piece CreatePieceOf(PieceType pieceType, IBoardPosition BoardPosition);
}