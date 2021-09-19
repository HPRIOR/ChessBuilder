using Models.Services.Moves.Interfaces;
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
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                var tile = board[i][j];
                var currentPiece = tile.CurrentPiece;
                if (currentPiece.Type != PieceType.NullPiece)
                    _pieceFactory.CreatePiece(currentPiece.Type, tile.Position);
            }
        }
    }
}