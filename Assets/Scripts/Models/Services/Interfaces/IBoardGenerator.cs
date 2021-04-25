using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IBoardGenerator
    {
        ITile[,] GenerateBoard();
    }
}