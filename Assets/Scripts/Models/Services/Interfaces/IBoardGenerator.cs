using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IBoardGenerator
    {
        Tile[,] GenerateBoard();
    }
}