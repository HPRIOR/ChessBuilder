using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using View.Interfaces;

namespace View.Renderers
{
    public class PieceRenderer : IPieceRenderer
    {
        private readonly IPieceSpawner _pieceSpawner;

        public PieceRenderer(IPieceSpawner pieceSpawner)
        {
            _pieceSpawner = pieceSpawner;
        }

        public void RenderPieces(BoardState previousState, BoardState newState)
        {
            DestroyExistingPieces();
            var board = newState.Board;
            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                if (currentPiece.Type != PieceType.NullPiece)
                    _pieceSpawner.CreatePiece(currentPiece.Type, tile.Position);
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