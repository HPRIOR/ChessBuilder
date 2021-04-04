public interface IPositionTranslator
{
    IBoardPosition GetRelativePosition(IBoardPosition originalPosition);

    ITile GetRelativeTileAt(IBoardPosition boardPosition, IBoardState fromBoard);
}