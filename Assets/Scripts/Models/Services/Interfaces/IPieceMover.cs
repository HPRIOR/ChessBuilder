using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IPieceMover
    {
        IBoardState GenerateNewBoardState(IBoardState originalBoardState, IBoardPosition from,
            IBoardPosition toDestination);
    }
}