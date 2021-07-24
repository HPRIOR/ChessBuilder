using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using View.Prefab.Interfaces;
using View.Prefab.Spawners;
using View.Utils;

namespace View.Prefab.Factories
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