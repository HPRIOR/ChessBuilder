using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Controllers.PieceMovers
{
    public class PieceMover : IPieceMover
    {
        public BoardState GenerateNewBoardState(BoardState originalBoardState, BoardPosition from,
            BoardPosition destination)
        {
            var newBoardState = (BoardState) originalBoardState.Clone();
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