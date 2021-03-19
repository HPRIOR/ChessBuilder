public interface IPieceMoveGeneratorFactory
{
    IPossibleMoveGenerator GetPossibleMoveGenerator(PieceType pieceType);
}