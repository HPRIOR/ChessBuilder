using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IPositionTranslator
    {
        IBoardPosition GetRelativePosition(IBoardPosition originalPosition);

        ITile GetRelativeTileAt(IBoardPosition boardPosition, IBoardState fromBoard);
    }
}