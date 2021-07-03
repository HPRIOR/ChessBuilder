using Models.State.Board;

namespace View.Interfaces
{
    public interface IStateChangeRenderer
    {
        void Render(BoardState previousState, BoardState newState);
    }
}