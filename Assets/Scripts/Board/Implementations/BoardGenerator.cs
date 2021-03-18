public class BoardGenerator : IBoardGenerator
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

    public ITile[,] RotateBoard(ITile[,] board)
    {
        var result = new Tile[8, 8];

        for (int iEnd = 8, iStart = 0; iStart < 8; iStart++, iEnd--)
            for (int jEnd = 8, jStart = 0; jEnd < 8; jStart++, jEnd--)
                result[iStart, jStart] = board[iEnd, jEnd] as Tile;

        return result;
    }
}