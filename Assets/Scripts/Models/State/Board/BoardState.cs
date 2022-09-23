using System;
using System.Collections.Generic;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.Board
{
    public sealed class BoardState
    {
        public readonly Tile[] Board;

        private static int Index(int x, int y) => x * 8 + y;


        public BoardState(Tile[] board)
        {
            Board = board;
            GenerateActivePieces();
        }

        // slow
        public BoardState(Dictionary<Position, (PieceType pieceType, BuildTileState buildTileState)> pieceMap)
        {
            var board = new Tile[64];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var position = new Position(i, j);
                    var piece = pieceMap.ContainsKey(position) ? pieceMap[position].pieceType : PieceType.NullPiece;
                    var buildTileState = pieceMap.ContainsKey(position)
                        ? pieceMap[position].buildTileState
                        : new BuildTileState(piece);

                    board[Index(i, j)] = new Tile(
                        new Position(i, j),
                        piece,
                        buildTileState
                    );
                }
            }

            Board = board;
            GenerateActivePieces();
        }

        public BoardState(Dictionary<Position, PieceType> pieceMap)
        {
            var board = new Tile[64];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var position = new Position(i, j);
                    var piece = pieceMap.ContainsKey(position) ? pieceMap[position] : PieceType.NullPiece;

                    board[Index(i, j)] = new Tile(
                        new Position(i, j),
                        piece
                    );
                }
            }

            Board = board;
            GenerateActivePieces();
        }


        private BoardState(Tile[] board, List<Position> activePieces, List<Position> activeBuilds)
        {
            Board = board;
            ActivePieces = activePieces;
            ActiveBuilds = activeBuilds;
        }

        public BoardState()
        {
            var board = new Tile[64];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                    board[Index(i, j)] = new Tile(
                        new Position(i, j)
                    );
            }

            Board = board;
        }

        public List<Position> ActivePieces { get; private set; }
        public List<Position> ActiveBuilds { get; private set; }
        public Tile GetTileAt(Position pos) => Board[Index(pos.X, pos.Y)];
        public Tile GetTileAt(int x, int y) => Board[Index(x, y)];


        private void GenerateActivePieces()
        {
            ActivePieces = new List<Position>();
            ActiveBuilds = new List<Position>();

            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            { 
                var tile =  GetTileAt(i, j);
                if (tile.CurrentPiece != PieceType.NullPiece) ActivePieces.Add(tile.Position);

                if (tile.BuildTileState.BuildingPiece != PieceType.NullPiece) ActiveBuilds.Add(tile.Position);
            }
        }

        public BoardState WithBuild(Position buildPosition, PieceType piece, PieceColour turn)
        {
            var newBoard = new Tile[64];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var index = Index(i, j);
                    if (buildPosition.X == i && buildPosition.Y == j)
                        newBoard[index] = Board[index].WithBuild(piece);
                    else
                        newBoard[index] = Board[index].WithDecrementedBuildState(turn);
                }
            }

            return new BoardState(newBoard);
        }

        public BoardState WithMove(Position from, Position destination, PieceColour turn)
        {
            var newBoard = new Tile[64];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var index = Index(i, j);
                    var movedPiece = Board[Index(from.X,from.Y)].CurrentPiece;
                    if (from.X == i && from.Y == j)
                        newBoard[index] = Board[index].WithPiece(PieceType.NullPiece, turn);
                    else if (destination.X == i && destination.Y == j)
                        newBoard[index] = Board[index].WithPiece(movedPiece, turn);
                    else
                        newBoard[index] = Board[index].WithDecrementedBuildState(turn);
                }
            }

            return new BoardState(newBoard);
        }

        public BoardState Clone()
        {
            var newBoard = new Tile[64];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                    newBoard[Index(i, j)] = Board[Index(i, j)].Clone();
            }

            return new BoardState(newBoard, new List<Position>(ActivePieces), new List<Position>(ActiveBuilds));
        }
    }
}