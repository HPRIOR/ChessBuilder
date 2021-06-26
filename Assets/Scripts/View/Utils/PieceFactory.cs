using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace View.Utils
{
    public class PieceFactory : IPieceFactory
    {
        private readonly PieceSpawner.Factory _pieceFactory;

        public PieceFactory(PieceSpawner.Factory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public PieceSpawner CreatePiece(PieceType pieceType, Position position)
        {
            var piece = _pieceFactory.Create(new PieceInfo(pieceType), position);
            return piece;
        }
    }
}