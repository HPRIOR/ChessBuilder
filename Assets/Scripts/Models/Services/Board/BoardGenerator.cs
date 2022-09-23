using Models.State.Board;

namespace Models.Services.Board
{
    public sealed class BoardGenerator : IBoardGenerator
    {
        public Tile[] GenerateBoard()
        {
            var board = new Tile[64];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                    board[i * 8 + j] = new Tile(
                        new Position(i, j)
                    );
            }

            return board;
        }
    }
}