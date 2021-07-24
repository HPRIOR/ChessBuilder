using Models.State.Board;

namespace Models.Services.Board
{
    public interface IBoardGenerator
    {
        Tile[,] GenerateBoard();
    }
}