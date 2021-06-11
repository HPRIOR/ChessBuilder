using System;

namespace Models.State.Board
{
    public class BoardState : ICloneable
    {
        public BoardState(Tile[,] board)
        {
            Board = board;
        }

        public BoardState()
        {
            var board = new Tile[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                board[i, j] = new Tile(
                    new BoardPosition(i, j)
                );
            Board = board;
        }

        public Tile[,] Board { get; }

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