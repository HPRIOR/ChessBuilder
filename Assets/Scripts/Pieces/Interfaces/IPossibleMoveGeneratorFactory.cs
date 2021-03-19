public interface IPossibleMoveGeneratorFactory
{
    IPossibleMoveGenerator GetPossibleMoveGenerator(PieceType pieceType);
}