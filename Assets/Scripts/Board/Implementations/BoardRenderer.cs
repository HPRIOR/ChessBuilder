using UnityEngine;
using Zenject;

// 'View class'  which is subscribed to changes in game state
public class BoardRenderer : MonoBehaviour, IBoardRenderer
{
    public GameObject tilePrefab;
    private IPieceSpawner _pieceSpawner;
    private IGameState _gameState;

    private void Awake()
    {
        RenderBoard();
        _gameState.GameStateChangeEvent += RenderPieces;
    }

    [Inject]
    public void Construct(IPieceSpawner pieceSpawner, IGameState gameState)
    {
        _pieceSpawner = pieceSpawner;
        _gameState = gameState;
    }

    public void RenderBoard()
    {
        var board = CreateBoardPositions();
        var boardParent = new GameObject("BoardRender");
        var lightDarkColourSwitch = true;
        var greenColour = new Color32(118, 150, 86, 255);
        var creamColour = new Color32(238, 238, 210, 255);
        int count = 1;
        foreach (var boardTile in board)
        {
            // Instantiate tile prefab under BoardRender GameObject
            var currentTile = GameObject.Instantiate(tilePrefab);
            currentTile.transform.parent = boardParent.transform;
            currentTile.transform.position = new Vector2(boardTile.Item1, boardTile.Item2);

            // Change light and dark squares
            var spriteRenderer = currentTile.GetComponent<SpriteRenderer>();
            spriteRenderer.color = lightDarkColourSwitch ? greenColour : creamColour;

            // ensures first tile on the next row is the same colour last tile in row
            if (count % 8 != 0)
            {
                lightDarkColourSwitch = !lightDarkColourSwitch;
            }
            count += 1;
        }
    }

    private void RenderPieces(IBoardState previousState, IBoardState newState)
    {
        DestroyExistingPieces();
        var board = newState.Board;
        foreach (var tile in board)
        {
            var currentPiece = tile.CurrentPiece;
            if (currentPiece != PieceType.NullPiece)
                _pieceSpawner.CreatePiece(currentPiece, tile.BoardPosition);
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
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                board[i, j] = (i + 0.5f, j + 0.5f);
        return board;
    }
}