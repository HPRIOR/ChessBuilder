using UnityEngine;

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
        var result = new ITile[8, 8];

        for (int iEnd = 7, iStart = 0; iStart < 8; iStart++, iEnd--)
            for (int jEnd = 7, jStart = 0; jStart < 8; jStart++, jEnd--)
                result[iStart, jStart] = board[iEnd, jEnd];
        return result;
    }
}