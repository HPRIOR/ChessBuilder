public interface IPieceMoveGeneratorFactory
{
    IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType);
}