using Models.State.Board;

namespace Controllers.Interfaces
{
    public interface IPieceMover
    {
        BoardState GenerateNewBoardState(BoardState originalBoardState, Position from,
            Position toDestination);
    }
}