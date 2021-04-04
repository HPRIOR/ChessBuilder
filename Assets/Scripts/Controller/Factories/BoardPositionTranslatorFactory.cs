public class BoardPositionTranslatorFactory : IPositionTranslatorFactory
{
    private readonly PositionTranslator.Factory _boardPositionTranslatorFactory;

    public BoardPositionTranslatorFactory(PositionTranslator.Factory boardPositionTranslatorFactory)
    {
        _boardPositionTranslatorFactory = boardPositionTranslatorFactory;
    }

    public IPositionTranslator Create(PieceColour pieceColour) =>
        _boardPositionTranslatorFactory.Create(pieceColour);
}