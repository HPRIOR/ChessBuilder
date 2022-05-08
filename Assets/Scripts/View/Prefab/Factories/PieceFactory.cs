using Models.Services.Game.Implementations;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;
using UnityEngine;
using View.NetworkUserInput;
using View.Prefab.Interfaces;
using View.Prefab.Spawners;
using View.Utils;
using Zenject;

namespace View.Prefab.Factories
{
    public class PieceFactory : IPieceFactory
    {
        private readonly PieceSpawner.Factory _pieceFactory;
        private GameContext _context;

        public PieceFactory(PieceSpawner.Factory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public IPieceSpawner CreatePiece(PieceType pieceType, Position position)
        {
            var piece = _pieceFactory.Create(new PieceRenderInfo(pieceType), position);
            if (piece.RenderInfo.PieceType.Colour() != _context.PlayerColour)
                Object.Destroy(piece.gameObject.GetComponent<DragAndDropNetwork>());
            return piece;
        }

        [Inject]
        public void Construct(GameContext context)
        {
            _context = context;
        }
    }
}