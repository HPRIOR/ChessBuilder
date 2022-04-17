using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IPositionTranslator
    {
        Position GetRelativePosition(Position originalPosition);

        ref Tile GetRelativeTileAt(Position position, BoardState fromBoard);
    }
}