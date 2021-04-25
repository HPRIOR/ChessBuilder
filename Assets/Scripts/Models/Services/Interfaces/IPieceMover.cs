using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IPieceMover
    {
        IBoardState Move(IBoardState board, IBoardPosition from, IBoardPosition toDestination);
    }
}