using Models.Services.Interfaces;
using Models.State.Interfaces;
using Models.State.Piece;

namespace Models.Services.Moves.PieceMovers
{
    public class PieceMover : IPieceMover
    {
        public IBoardState GenerateNewBoardState(IBoardState originalBoard, IBoardPosition from, IBoardPosition destination)
        {
            var newBoard = (IBoardState)originalBoard.Clone();
            var destinationTile = newBoard.GetTileAt(destination);
            var fromTile = newBoard.GetTileAt(from);

            destinationTile.CurrentPiece = originalBoard.GetTileAt(from).CurrentPiece;
            fromTile.CurrentPiece = new State.Piece.Piece(PieceType.NullPiece);
            return newBoard;
        }
    }
}