using Models.State.Board;

namespace Models.Services.Board
{
    public class BoardGenerator : IBoardGenerator
    {
        public Tile[][] GenerateBoard()
        {
            var board = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                board[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                    board[i][j] = new Tile(
                        new Position(i, j)
                    );
            }

            return board;
        }
    }
}