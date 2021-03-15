using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    public IBoardPosition BoardPosition { get; set; }
    public PieceColour PieceColour { get; set; }
    public PieceType PieceType { get; set; }
    
    private ICommandInvoker _commandInvoker;
    // Todo: make the PieceSpriteAssetManager a scriptable object 
    private static IDictionary<PieceType, string> _spriteAssetMap = PieceSpriteAssetManager.PieceSpriteMap; 
    private bool _isDragging;
    private SpriteRenderer _spriteRenderer;
    private static MovePieceCommandFactory _movePieceCommandFactory;

    private void Start()
    {
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(_spriteAssetMap[PieceType]);
        gameObject.transform.position = BoardPosition.Position;
    }

    [Inject]
    public void Construct(
        PieceType pieceType,
        IBoardPosition boardPosition, 
        ICommandInvoker commandInvoker, 
        MovePieceCommandFactory movePieceCommandFactory)
    {
        PieceType = pieceType;
        BoardPosition = boardPosition;
        _commandInvoker = commandInvoker;
        _movePieceCommandFactory = movePieceCommandFactory;

    }
    private void OnMouseDown()
    {
        _spriteRenderer.sortingOrder = 2;
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _spriteRenderer.sortingOrder = 1;
        var currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var nearestBoardPosition = GetNearestBoardPosition(currentMousePosition);

        _commandInvoker.AddCommand(_movePieceCommandFactory.Create(gameObject, nearestBoardPosition));
        _isDragging = false;
    }

    private void Update()
    {
        if (_isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }

    private IBoardPosition GetNearestBoardPosition(Vector2 position) =>
        new BoardPosition(ConvertAxisToNearestBoardIndex(position.x), ConvertAxisToNearestBoardIndex(position.y));

    private int ConvertAxisToNearestBoardIndex(float axis)
    {
        if (axis > 7.5) return 7;
        if (axis < 0.5) return 0;
        return (int)axis;
    }

    public class Factory : PlaceholderFactory<PieceType, IBoardPosition, Piece> { }

}