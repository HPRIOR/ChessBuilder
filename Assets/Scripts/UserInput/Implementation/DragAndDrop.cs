using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DragAndDrop : MonoBehaviour
{

    private ICommandInvoker _commandInvoker;
    private bool _isDragging;
    private static MovePieceCommandFactory _movePieceCommandFactory;
    [Inject]
    public void Construct(
        ICommandInvoker commandInvoker,
        MovePieceCommandFactory movePieceCommandFactory)
    {
        _commandInvoker = commandInvoker;
        _movePieceCommandFactory = movePieceCommandFactory;

    }
    private void OnMouseDown()
    {
        _isDragging = true;
    }
    
    private void OnMouseUp()
    {
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


}
