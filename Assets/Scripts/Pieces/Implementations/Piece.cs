using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    private Dictionary<PieceType, string> spriteAssetMap = new Dictionary<PieceType, string>()
    {
        {PieceType.BlackPawn, "Assets/Sprites/ChessPieces/45x45/45px-Chess_pdt45.svg.png"},
        {PieceType.BlackBishop,"Assets/Sprites/ChessPieces/45x45/45px-Chess_bdt45.svg.png" },
        {PieceType.BlackKnight, "Assets/Sprites/ChessPieces/45x45/45px-Chess_ndt45.svg.png"},
        {PieceType.BlackRook, "Assets/Sprites/ChessPieces/45x45/45px-Chess_rdt45.svg.png" },
        {PieceType.BlackKing, "Assets/Sprites/ChessPieces/45x45/45px-Chess_kdt45.svg.png"},
        {PieceType.BlackQueen, "Assets/Sprites/ChessPieces/45x45/45px-Chess_qdt45.svg.png"},
        {PieceType.WhitePawn, "Assets/Sprites/ChessPieces/45x45/45px-Chess_plt45.svg.png"},
        {PieceType.WhiteBishop, "Assets/Sprites/ChessPieces/45x45/45px-Chess_blt45.svg.png"},
        {PieceType.WhiteKnight, "Assets/Sprites/ChessPieces/45x45/45px-Chess_nlt45.svg.png"},
        {PieceType.WhiteRook, "Assets/Sprites/ChessPieces/45x45/45px-Chess_rlt45.svg.png"},
        {PieceType.WhiteKing, "Assets/Sprites/ChessPieces/45x45/45px-Chess_klt45.svg.png"},
        {PieceType.WhiteQueen, "Assets/Sprites/ChessPieces/45x45/45px-Chess_qlt45.svg.png"}
    };

    public IBoardPosition BoardPosition { get; set; }
    public PieceColour PieceColour { get; set; }
    public PieceType PieceType { get; set; }
    private ICommandInvoker _commandInvoker;
    private bool _isDragging;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Pieces").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteAssetMap[PieceType]);
        gameObject.transform.position = BoardPosition.Position;

    }

    [Inject]
    public void Construct(PieceType pieceType, IBoardPosition boardPosition, ICommandInvoker commandInvoker)
    {
        PieceType = pieceType;
        BoardPosition = boardPosition;
        _commandInvoker = commandInvoker;

    }
    public void OnMouseDown()
    {
        _spriteRenderer.sortingOrder = 2;
        _isDragging = true;
    }

    public void OnMouseUp()
    {
        _spriteRenderer.sortingOrder = 1;
        var currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var nearestBoardPosition = GetNearestBoardPosition(currentMousePosition);

        _commandInvoker.AddCommand(new MovePieceCommand(gameObject, nearestBoardPosition));
        _isDragging = false;
    }

    public void Update()
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