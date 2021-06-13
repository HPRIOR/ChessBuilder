using Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using View.Interfaces;
using Zenject;

// 'View class'  which is subscribed to changes in game state
namespace View.Renderers
{
    public class BoardRenderer : MonoBehaviour, IBoardRenderer
    {
        public GameObject tilePrefab;
        private IPieceSpawner _pieceSpawner;
        private ITurnEventInvoker _turnEventInvoker;

        private void Awake()
        {
            RenderBoard();
            _turnEventInvoker.GameStateChangeEvent += RenderPieces;
        }

        public void RenderBoard()
        {
            var board = CreateBoardPositions();
            var boardParent = new GameObject("BoardRender");
            var lightDarkColourSwitch = true;
            var greenColour = new Color32(118, 150, 86, 255);
            var creamColour = new Color32(238, 238, 210, 255);
            var count = 1;
            foreach (var (x, y) in board)
            {
                // Instantiate tile prefab under BoardRender GameObject
                var currentTile = Instantiate(tilePrefab, boardParent.transform, true);
                currentTile.transform.position = new Vector2(x, y);

                // Change light and dark squares
                var spriteRenderer = currentTile.GetComponent<SpriteRenderer>();
                spriteRenderer.color = lightDarkColourSwitch ? greenColour : creamColour;

                // ensures first tile on the next row is the same colour last tile in row
                if (count % 8 != 0) lightDarkColourSwitch = !lightDarkColourSwitch;
                count += 1;
            }
        }

        [Inject]
        public void Construct(IPieceSpawner pieceSpawner, ITurnEventInvoker turnEventInvoker)
        {
            _pieceSpawner = pieceSpawner;
            _turnEventInvoker = turnEventInvoker;
        }

        private void RenderPieces(BoardState previousState, BoardState newState)
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

        private void DestroyExistingPieces()
        {
            var piecesGameObject = GameObject.FindGameObjectWithTag("Pieces");
            if (piecesGameObject.transform.childCount > 0)
                foreach (Transform child in piecesGameObject.transform)
                    Destroy(child.gameObject);
        }

        private (float X, float Y)[,] CreateBoardPositions()
        {
            var board = new (float X, float Y)[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                board[i, j] = (i + 0.5f, j + 0.5f);
            return board;
        }
    }
}