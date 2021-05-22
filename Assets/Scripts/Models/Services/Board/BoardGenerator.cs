using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;

namespace Models.Services.Board
{
    public class BoardGenerator : IBoardGenerator
    {
        public ITile[,] GenerateBoard()
        {
            Tile[,] board = new Tile[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                board[i, j] = new Tile(
                    new BoardPosition(i, j)
                );
            }
            return board;
        }
    }
}