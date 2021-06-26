using Models.State.Board;

namespace View.Interfaces
{
    public interface IPieceRenderer
    {
        void RenderPieces(BoardState previousState, BoardState newState);
    }
}