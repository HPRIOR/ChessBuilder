using System;
using System.Collections.Generic;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.State.Board
{
    public sealed class BoardState
    {
        public readonly Tile[][] Board;
        public ref Tile GetTileAt(Position pos) => ref Board[pos.X][pos.Y];
        public ref Tile GetTileAt(int x, int y) => ref Board[x][y];

        public BoardState(Tile[][] board)
        {
            Board = board;
            ActivePieces = new List<Position>();
            ActiveBuilds = new List<Position>();
     
            GenerateActivePieces();
        }

        private BoardState(Tile[][] board, List<Position> activePieces, List<Position> activeBuilds)
        {
            Board = board;
            ActivePieces = activePieces;
            ActiveBuilds = activeBuilds;
        }

        public BoardState()
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

            Board = board;
        }

        public List<Position> ActivePieces { get; }
        public List<Position> ActiveBuilds { get; }
        

        private void GenerateActivePieces()
        {
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                ref var tile = ref GetTileAt(i,j);
                if (tile.CurrentPiece.Type != PieceType.NullPiece)
                {
                    ActivePieces.Add(tile.Position);
                }

                if (tile.BuildTileState.BuildingPiece != PieceType.NullPiece)
                {
                    ActiveBuilds.Add(tile.Position);
                }
            }
        }

        public BoardState Clone()
        {
            var newBoard = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                newBoard[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                    newBoard[i][j] = Board[i][j].Clone();
            }

            return new BoardState(newBoard, new List<Position>(ActivePieces), new List<Position>(ActiveBuilds));
        }

        public BoardState CloneWithDecrementBuildState()
        {
            var newBoard = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                newBoard[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                    newBoard[i][j] = Board[i][j].CloneWithDecrementBuildState();
            }

            return new BoardState(newBoard, new List<Position>(ActivePieces), new List<Position>(ActiveBuilds));
        }
    }
}