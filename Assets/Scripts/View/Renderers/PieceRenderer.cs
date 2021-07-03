using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using View.Interfaces;
using View.Utils;

namespace View.Renderers
{
    public class PieceRenderer : IStateChangeRenderer
    {
        private readonly IPieceFactory _pieceFactory;

        public PieceRenderer(IPieceFactory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public void Render(BoardState previousState, BoardState newState)
        {
            GameObjectDestroyer.DestroyChildrenOfObjectWith("Pieces");
            var board = newState.Board;
            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                if (currentPiece.Type != PieceType.NullPiece)
                    _pieceFactory.CreatePiece(currentPiece.Type, tile.Position);
            }
        }
    }
}