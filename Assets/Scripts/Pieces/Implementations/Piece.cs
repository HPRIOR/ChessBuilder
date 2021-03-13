﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Piece : MonoBehaviour
{
    private static IDictionary<PieceType, string> _spriteAssetMap = PieceSpriteAssetManager.PieceSpriteMap; 
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
        _spriteRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(_spriteAssetMap[PieceType]);
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