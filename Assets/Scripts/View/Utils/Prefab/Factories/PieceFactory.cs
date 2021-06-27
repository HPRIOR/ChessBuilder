using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using View.Utils.Prefab.Interfaces;
using View.Utils.Prefab.Spawners;

namespace View.Utils.Prefab.Factories
{
    public class PieceFactory : IPieceFactory
    {
        private readonly PieceSpawner.Factory _pieceFactory;

        public PieceFactory(PieceSpawner.Factory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public IPieceSpawner CreatePiece(PieceType pieceType, Position position)
        {
            var piece = _pieceFactory.Create(new PieceRenderInfo(pieceType), position);
            return piece;
        }
    }
}