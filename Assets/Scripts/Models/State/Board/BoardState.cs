using System.Collections.Generic;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.Board
{
    public sealed class BoardState
    {
        public readonly Tile[][] Board;

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
        public ref Tile GetTileAt(Position pos) => ref Board[pos.X][pos.Y];
        public ref Tile GetTileAt(int x, int y) => ref Board[x][y];


        private void GenerateActivePieces()
        {
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                ref var tile = ref GetTileAt(i, j);
                if (tile.CurrentPiece != PieceType.NullPiece)
                {
                    ActivePieces.Add(tile.Position);
                }

                if (tile.BuildTileState.BuildingPiece != PieceType.NullPiece)
                {
                    ActiveBuilds.Add(tile.Position);
                }
            }
        }

        public BoardState WithBuild(Position buildPosition, PieceType piece, PieceColour turn)
        {
            var newBoard = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                newBoard[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                {
                    if (buildPosition.X == i && buildPosition.Y == j)
                        newBoard[i][j] = Board[i][j].WithBuild(piece);
                    else
                        newBoard[i][j] = Board[i][j].WithDecrementedBuildState(turn);
                }
            }

            return new BoardState(newBoard);
        }

        public BoardState WithMove(Position from, Position destination, PieceColour turn)
        {
            var newBoard = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                newBoard[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                {
                    var movedPiece = Board[from.X][from.Y].CurrentPiece;
                    if (from.X == i && from.Y == j)
                    {
                        newBoard[i][j] = Board[i][j].WithPiece(PieceType.NullPiece, turn);
                    }
                    else if (destination.X == i && destination.Y == j)
                    {
                        newBoard[i][j] = Board[i][j].WithPiece(movedPiece, turn);
                    }
                    else
                    {
                        newBoard[i][j] = Board[i][j].WithDecrementedBuildState(turn);
                    }
                }
            }

            return new BoardState(newBoard);
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
    }
}