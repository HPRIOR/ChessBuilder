using Models.Services.Interfaces;
using Models.State.Interfaces;
using Models.State.PieceState;

namespace Controllers.PieceMovers
{
    public class PieceMover : IPieceMover
    {
        public IBoardState GenerateNewBoardState(IBoardState originalBoard, IBoardPosition from,
            IBoardPosition destination)
        {
            var newBoard = (IBoardState) originalBoard.Clone();
            var destinationTile = newBoard.GetTileAt(destination);
            var fromTile = newBoard.GetTileAt(from);

            destinationTile.CurrentPiece = originalBoard.GetTileAt(from).CurrentPiece;
            fromTile.CurrentPiece = new Piece(PieceType.NullPiece);
            return newBoard;
        }
    }
}