﻿using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private CommandInvoker _commandInvoker;
    private bool _isDragging;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _commandInvoker = GameObject.FindGameObjectWithTag("InputSystem").GetComponent<CommandInvoker>();
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

        _commandInvoker.AddCommand(new MovePieceCommand(gameObject, nearestBoardPosition));
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
}