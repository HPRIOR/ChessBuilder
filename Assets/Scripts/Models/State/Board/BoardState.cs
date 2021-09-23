using System;
using System.Collections.Generic;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.State.Board
{
    public class BoardState
    {
        public readonly Tile[][] Board;
        public ref Tile GetTileAt(Position pos) => ref Board[pos.X][pos.Y];
        public ref Tile GetTileAt(int x, int y) => ref Board[x][y];

        public BoardState(Tile[][] board)
        {
            Board = board;
            ActivePieces = new HashSet<Position>();
            ActiveBuilds = new HashSet<Position>();
            ActiveBlackBuilds = new HashSet<Position>();
            ActiveWhiteBuilds = new HashSet<Position>();
            ActiveBlackPieces = new HashSet<Position>();
            ActiveWhitePieces = new HashSet<Position>();
            GenerateActivePieces();
        }

        private BoardState(Tile[][] board, HashSet<Position> activePieces, HashSet<Position> activeBuilds,
            HashSet<Position> activeBlackPieces, HashSet<Position> activeWhitePieces,
            HashSet<Position> activeBlackBuilds, HashSet<Position> activeWhiteBuilds)
        {
            Board = board;
            ActivePieces = activePieces;
            ActiveBuilds = activeBuilds;
            ActiveBlackPieces = activeBlackPieces;
            ActiveWhitePieces = activeWhitePieces;
            ActiveBlackBuilds = activeBlackBuilds;
            ActiveWhiteBuilds = activeWhiteBuilds;
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

        public HashSet<Position> ActivePieces { get; }
        public HashSet<Position> ActiveBuilds { get; }
        public HashSet<Position> ActiveBlackPieces { get; }
        public HashSet<Position> ActiveWhitePieces { get; }
        public HashSet<Position> ActiveBlackBuilds { get; }
        public HashSet<Position> ActiveWhiteBuilds { get; }

        private void GenerateActivePieces()
        {
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                ref var tile = ref GetTileAt(i,j);
                if (tile.CurrentPiece.Type != PieceType.NullPiece)
                {
                    ActivePieces.Add(tile.Position);
                    if (tile.CurrentPiece.Colour == PieceColour.Black)
                        ActiveBlackPieces.Add(tile.Position);
                    else
                        ActiveWhitePieces.Add(tile.Position);
                }

                if (tile.BuildTileState.BuildingPiece != PieceType.NullPiece)
                {
                    ActiveBuilds.Add(tile.Position);
                    if (tile.BuildTileState.BuildingPiece.Colour() == PieceColour.Black)
                        ActiveBlackBuilds.Add(tile.Position);
                    else
                        ActiveWhiteBuilds.Add(tile.Position);
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

            return new BoardState(newBoard, new HashSet<Position>(ActivePieces), new HashSet<Position>(ActiveBuilds),
                new HashSet<Position>(ActiveBlackPieces), new HashSet<Position>(ActiveWhitePieces),
                new HashSet<Position>(ActiveBlackBuilds), new HashSet<Position>(ActiveWhiteBuilds));
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

            return new BoardState(newBoard, new HashSet<Position>(ActivePieces), new HashSet<Position>(ActiveBuilds),
                new HashSet<Position>(ActiveBlackPieces), new HashSet<Position>(ActiveWhitePieces),
                new HashSet<Position>(ActiveBlackBuilds), new HashSet<Position>(ActiveWhiteBuilds));
        }
    }
}