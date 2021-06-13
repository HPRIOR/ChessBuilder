using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IPositionTranslator
    {
        Position GetRelativePosition(Position originalPosition);

        Tile GetRelativeTileAt(Position position, BoardState fromBoard);
    }
}