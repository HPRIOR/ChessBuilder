using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IPieceMover
    {
        BoardState GenerateNewBoardState(BoardState originalBoardState, Position from,
            Position toDestination);
    }
}