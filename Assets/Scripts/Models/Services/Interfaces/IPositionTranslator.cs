using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IPositionTranslator
    {
        BoardPosition GetRelativePosition(BoardPosition originalPosition);

        Tile GetRelativeTileAt(BoardPosition boardPosition, BoardState fromBoard);
    }
}