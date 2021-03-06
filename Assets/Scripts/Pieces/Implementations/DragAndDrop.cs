using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DragAndDrop : MonoBehaviour 
{
    private bool _isDragging;
    private (float, float)[,] _boardPositions = new (float, float)[8,8];

    public void Start()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                _boardPositions[i, j] = (i + 0.5f, j + 0.5f);
    }

    public void OnMouseDown()
    {
        _isDragging = true;
    }
    public void OnMouseUp()
    {
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

}
