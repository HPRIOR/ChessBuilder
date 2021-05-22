using System;
using Models.Services.Interfaces;
using Models.State.Interfaces;

namespace Models.State.Board
{
    public class BoardState : IBoardState 
    {
        public ITile[,] Board { get; }

        public BoardState(ITile[,] board)
        {
            Board = board;
        }

        public BoardState(IBoardGenerator boardGenerator)
        {
            Board = boardGenerator.GenerateBoard();
        }

        public ITile GetTileAt(IBoardPosition boardPosition) =>
            Board[boardPosition.X, boardPosition.Y];

        public ITile GetMirroredTileAt(IBoardPosition boardPosition) =>
            Board[Math.Abs(boardPosition.X - 7), Math.Abs(boardPosition.Y - 7)];

        public object Clone()
        {
            var newBoard = new Tile[8, 8];
            for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                newBoard[i, j] = (Tile)Board[i, j].Clone();
            }
            return new BoardState(newBoard);
        }
    }
}