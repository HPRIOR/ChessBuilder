using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;


public class BoardRenderer : IBoardRenderer
{
    public void RenderBoard(ITile[,] board, GameObject tilePrefab)
    {
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
            currentTile.transform.position = boardTile.BoardPosition.Position;
            
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
}
