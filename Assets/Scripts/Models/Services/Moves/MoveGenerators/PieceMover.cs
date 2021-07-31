using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.MoveGenerators
{
    public class PieceMover : IPieceMover
    {
        public BoardState GenerateNewBoardState(BoardState originalBoardState, Position from,
            Position destination)
        {
            var newActivePieces = new HashSet<Position>(originalBoardState.ActivePieces);
            newActivePieces.Remove(from);
            newActivePieces.Add(destination);

            var newActiveBuilds = new HashSet<Position>(originalBoardState.ActiveBuilds);
            var newBoardState = originalBoardState.CloneWithDecrementBuildState(newActivePieces, newActiveBuilds);

            // get swapped pieces
            var destinationTile = newBoardState.Board[destination.X, destination.Y];
            var fromTile = newBoardState.Board[from.X, from.Y];

            // swap pieces
            destinationTile.CurrentPiece = originalBoardState.Board[from.X, from.Y].CurrentPiece;
            fromTile.CurrentPiece = new Piece(PieceType.NullPiece);

            return newBoardState;
        }
    }
}