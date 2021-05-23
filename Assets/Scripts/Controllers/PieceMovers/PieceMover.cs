using Models.Services.Interfaces;
using Models.State.Interfaces;
using Models.State.PieceState;

namespace Controllers.PieceMovers
{
    public class PieceMover : IPieceMover
    {
        public IBoardState GenerateNewBoardState(IBoardState originalBoardState, IBoardPosition from,
            IBoardPosition destination)
        {
            var newBoardState = (IBoardState) originalBoardState.Clone();
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