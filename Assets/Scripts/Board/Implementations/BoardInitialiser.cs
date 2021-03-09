public class BoardInitialiser : IBoardGenerator
{
    public ITile[,] GenerateBoard()
    {
        var board = new Tile[8, 8];
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = new Tile(
                    new BoardPosition(i, j)
                    );
            }

        return board;
    }
}