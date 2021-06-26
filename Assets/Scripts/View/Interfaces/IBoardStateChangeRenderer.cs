using Models.State.Board;

namespace View.Interfaces
{
    public interface IBoardStateChangeRenderer
    {
        void Render(BoardState previousState, BoardState newState);
    }
}