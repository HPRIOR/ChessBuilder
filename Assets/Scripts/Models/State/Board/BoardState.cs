using System.Collections.Generic;

namespace Models.State.Board
{
    public class BoardState
    {
        public BoardState(Tile[,] board)
        {
            Board = board;
        }

        public BoardState(Tile[,] board, HashSet<Position> activePieces, HashSet<Position> activeBuilds)
        {
            Board = board;
            ActivePieces = activePieces;
            ActiveBuilds = activeBuilds;
        }

        public BoardState()
        {
            var board = new Tile[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                board[i, j] = new Tile(
                    new Position(i, j)
                );
            Board = board;
        }


        public Tile[,] Board { get; }

        public HashSet<Position> ActivePieces { get; }
        public HashSet<Position> ActiveBuilds { get; }

        public BoardState Clone()
        {
            var newBoard = new Tile[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                newBoard[i, j] = Board[i, j].Clone();
            return new BoardState(newBoard);
        }

        public BoardState CloneWithDecrementBuildState()
        {
            var newBoard = new Tile[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                newBoard[i, j] = Board[i, j].CloneWithDecrementBuildState();
            return new BoardState(newBoard, new HashSet<Position>(ActivePieces), new HashSet<Position>(ActiveBuilds));
        }

        public BoardState CloneWithDecrementBuildState(HashSet<Position> activePieces, HashSet<Position> activeBuilds)
        {
            var newBoard = new Tile[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                newBoard[i, j] = Board[i, j].CloneWithDecrementBuildState();
            return new BoardState(newBoard, activePieces, activeBuilds);
        }
    }
}