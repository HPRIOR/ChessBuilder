public interface IPieceMover
{
    IBoardState Move(IBoardState board, IBoardPosition from, IBoardPosition toDestination);
}