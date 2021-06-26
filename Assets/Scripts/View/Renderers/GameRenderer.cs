using Game.Interfaces;
using UnityEngine;
using View.Interfaces;
using Zenject;

// 'View class'  which is subscribed to changes in game state
namespace View.Renderers
{
    public class GameRenderer : MonoBehaviour
    {
        public GameObject tilePrefab;
        private IBoardRenderer _boardRenderer;
        private IPieceRenderer _pieceRenderer;
        private ITurnEventInvoker _turnEventInvoker;

        private void Awake()
        {
            _boardRenderer.RenderBoard(tilePrefab);
            _turnEventInvoker.GameStateChangeEvent += _pieceRenderer.RenderPieces;
        }


        [Inject]
        public void Construct(IPieceRenderer pieceRenderer, IBoardRenderer boardRenderer,
            ITurnEventInvoker turnEventInvoker)
        {
            _pieceRenderer = pieceRenderer;
            _turnEventInvoker = turnEventInvoker;
            _boardRenderer = boardRenderer;
        }
    }
}