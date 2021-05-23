using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using View.Renderers;

namespace Models.Services.Piece
{
    public class PieceSpawner : IPieceSpawner
    {
        private readonly PieceMono.Factory _pieceFactory;

        public PieceSpawner(PieceMono.Factory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public PieceMono CreatePiece(PieceType pieceType, BoardPosition boardPosition)
        {
            var piece = _pieceFactory.Create(new PieceInfo(pieceType), boardPosition);
            piece.BoardPosition = boardPosition;
            return piece;
        }
    }
}