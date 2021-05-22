using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IPieceMover
    {
        IBoardState GenerateNewBoardState(IBoardState board, IBoardPosition from, IBoardPosition toDestination);
    }
}