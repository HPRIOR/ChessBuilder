using Models.Services.Interfaces;
using Models.State.Interfaces;

namespace Models.State.Board
{
    public class BoardState : IBoardState
    {
        public BoardState(ITile[,] board)
        {
            Board = board;
        }

        public BoardState(IBoardGenerator boardGenerator)
        {
            Board = boardGenerator.GenerateBoard();
        }

        public ITile[,] Board { get; }

        public object Clone()
        {
            var newBoard = new Tile[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                newBoard[i, j] = Board[i, j].Clone() as Tile;
            return new BoardState(newBoard);
        }
    }
}