public interface IPositionTranslatorFactory
{
    IPositionTranslator Create(PieceColour pieceColour);
}