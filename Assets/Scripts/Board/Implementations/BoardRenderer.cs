using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;


// separate this into another class that doesn't depend on tiles so it can be executed indipendantly
public class BoardRenderer : MonoBehaviour, IBoardRenderer
{
    public GameObject tilePrefab;
    private void Start()
    {
        RenderBoard();
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

    private (float X, float Y)[,] CreateBoardPositions()
    {
        var board = new (float X, float Y)[8, 8];
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                board[i, j] = (i + 0.5f, j + 0.5f);
        return board;

    }
}
