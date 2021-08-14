using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IPieceMover
    {
        void ModifyBoardState(BoardState boardState, Position from,
            Position toDestination);
    }
}