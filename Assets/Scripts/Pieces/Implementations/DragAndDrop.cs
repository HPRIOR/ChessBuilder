using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DragAndDrop : MonoBehaviour 
{
    private bool _isDragging;
    private PieceInfo _pieceInfo;
    
    private void Start()
    {
        _pieceInfo = GetComponent<PieceInfo>();
    }

    private void OnMouseDown()
    {
       _isDragging = true;
    }
    private void OnMouseUp()
    {
        var currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var nearestBoardPosition = GetNearestBoardPosition(currentMousePosition);
        SnapToNearestTile(nearestBoardPosition);
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

    private void SnapToNearestTile(IBoardPosition currentBoardPosition)
    {
        transform.position = currentBoardPosition.Position;
    }

}
