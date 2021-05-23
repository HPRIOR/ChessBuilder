using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IPieceMover
    {
        BoardState GenerateNewBoardState(BoardState originalBoardState, BoardPosition from,
            BoardPosition toDestination);
    }
}