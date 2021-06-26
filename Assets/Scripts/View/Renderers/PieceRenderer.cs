using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using View.Interfaces;

namespace View.Renderers
{
    public class PieceRenderer : IBoardStateChangeRenderer
    {
        private readonly IPieceFactory _pieceFactory;

        public PieceRenderer(IPieceFactory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        public void Render(BoardState previousState, BoardState newState)
        {
            DestroyExistingPieces();
            var board = newState.Board;
            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                if (currentPiece.Type != PieceType.NullPiece)
                    _pieceFactory.CreatePiece(currentPiece.Type, tile.Position);
            }
        }


        private static void DestroyExistingPieces()
        {
            var piecesGameObject = GameObject.FindGameObjectWithTag("Pieces");
            if (piecesGameObject.transform.childCount > 0)
                foreach (Transform child in piecesGameObject.transform)
                    Object.Destroy(child.gameObject);
        }
    }
}