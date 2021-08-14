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
            // modify active pieces 
            var newActivePieces = new HashSet<Position>(originalBoardState.ActivePieces);
            newActivePieces.Remove(from);
            newActivePieces.Add(destination);

            // do nothing to active builds 
            var newActiveBuilds = new HashSet<Position>(originalBoardState.ActiveBuilds);
            var newBoardState = originalBoardState.CloneWithDecrementBuildState(newActivePieces, newActiveBuilds);

            // modify board state
            var destinationTile = newBoardState.Board[destination.X, destination.Y];
            var fromTile = newBoardState.Board[from.X, from.Y];

            // swap pieces
            destinationTile.CurrentPiece = originalBoardState.Board[from.X, from.Y].CurrentPiece;
            fromTile.CurrentPiece = new Piece(PieceType.NullPiece);

            // return nothing 
            return newBoardState;
        }
    }
}