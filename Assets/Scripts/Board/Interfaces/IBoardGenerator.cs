public interface IBoardGenerator
{
    ITile[,] GenerateBoard();

    ITile[,] RotateBoard(ITile[,] board);
}