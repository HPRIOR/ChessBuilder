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
        private IPrefabRenderer _boardRenderer;
        private IBoardStateChangeRenderer _buildRenderer;
        private IBoardStateChangeRenderer _pieceRenderer;
        private ITurnEventInvoker _turnEventInvoker;

        private void Awake()
        {
            _boardRenderer.RenderBoard(tilePrefab);
            _turnEventInvoker.GameStateChangeEvent += _pieceRenderer.Render;
            _turnEventInvoker.GameStateChangeEvent += _buildRenderer.Render;
        }


        [Inject]
        public void Construct(
            [Inject(Id = "Piece")] IBoardStateChangeRenderer pieceRenderer,
            [Inject(Id = "Build")] IBoardStateChangeRenderer buildRenderer,
            IPrefabRenderer boardRenderer,
            ITurnEventInvoker turnEventInvoker)
        {
            _buildRenderer = buildRenderer;
            _pieceRenderer = pieceRenderer;
            _turnEventInvoker = turnEventInvoker;
            _boardRenderer = boardRenderer;
        }
    }
}