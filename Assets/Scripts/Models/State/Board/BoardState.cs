using System.Collections.Generic;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.Board
{
    public sealed class BoardState
    {

        public BoardState(Tile[][] board)
        {
            _board = board;
            GenerateActivePieces();
        }

        // slow
        public BoardState(Dictionary<Position, (PieceType pieceType, BuildTileState buildTileState)> pieceMap)
        {
            var board = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                board[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                {
                    var position = new Position(i, j);
                    var piece = pieceMap.ContainsKey(position) ? pieceMap[position].pieceType : PieceType.NullPiece;
                    var buildTileState = pieceMap.ContainsKey(position)
                        ? pieceMap[position].buildTileState
                        : new BuildTileState(piece);

                    board[i][j] = new Tile(
                        new Position(i, j),
                        piece,
                        buildTileState
                    );
                }
            }

            _board = board;
            GenerateActivePieces();
        }

        public BoardState(Dictionary<Position, PieceType> pieceMap)
        {
            var board = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                board[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                {
                    var position = new Position(i, j);
                    var piece = pieceMap.ContainsKey(position) ? pieceMap[position] : PieceType.NullPiece;

                    board[i][j] = new Tile(
                        new Position(i, j),
                        piece
                    );
                }
            }

            _board = board;
            GenerateActivePieces();
        }


        private BoardState(Tile[][] board, List<Position> activePieces, List<Position> activeBuilds)
        {
            _board = board;
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

            _board = board;
        }

        private readonly Tile[][] _board;
        public List<Position> ActivePieces { get; private set; }
        public List<Position> ActiveBuilds { get; private set; }
        public ref Tile GetTileAt(Position pos) => ref _board[pos.X][pos.Y];
        public ref Tile GetTileAt(int x, int y) => ref _board[x][y];


        private void GenerateActivePieces()
        {
            ActivePieces = new List<Position>();
            ActiveBuilds = new List<Position>();

            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                ref var tile = ref GetTileAt(i, j);
                if (tile.CurrentPiece != PieceType.NullPiece) ActivePieces.Add(tile.Position);

                if (tile.BuildTileState.BuildingPiece != PieceType.NullPiece) ActiveBuilds.Add(tile.Position);
            }
        }

        public BoardState WithBuild(Position buildPosition, PieceType piece, PieceColour turn)
        {
            var newBoard = new Tile[8][];
            for (var i = 0; i < 8; i++)
            {
                newBoard[i] = new Tile[8];
                for (var j = 0; j < 8; j++)
                    if (buildPosition.X == i && buildPosition.Y == j)
                        newBoard[i][j] = _board[i][j].WithBuild(piece);
                    else
                        newBoard[i][j] = _board[i][j].WithDecrementedBuildState(turn);
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
                    var movedPiece = _board[from.X][from.Y].CurrentPiece;
                    if (from.X == i && from.Y == j)
                        newBoard[i][j] = _board[i][j].WithPiece(PieceType.NullPiece, turn);
                    else if (destination.X == i && destination.Y == j)
                        newBoard[i][j] = _board[i][j].WithPiece(movedPiece, turn);
                    else
                        newBoard[i][j] = _board[i][j].WithDecrementedBuildState(turn);
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
                    newBoard[i][j] = _board[i][j].Clone();
            }

            return new BoardState(newBoard, new List<Position>(ActivePieces), new List<Position>(ActiveBuilds));
        }
    }
}