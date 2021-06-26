using UnityEngine;
using View.Interfaces;

namespace View.Renderers
{
    public class BoardRenderer : IBoardRenderer
    {
        public void RenderBoard(GameObject tilePrefab)
        {
            var board = CreateBoardPositions();
            var boardParent = new GameObject("BoardRender");
            var lightDarkColourSwitch = true;
            var greenColour = new Color32(118, 150, 86, 255);
            var creamColour = new Color32(238, 238, 210, 255);
            var count = 1;
            foreach (var (x, y) in board)
            {
                // Instantiate tile prefab under BoardRender GameObject
                var currentTileObject = Object.Instantiate(tilePrefab, boardParent.transform, true);
                currentTileObject.transform.position = new Vector2(x, y);

                // Change light and dark squares
                var spriteRenderer = currentTileObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = lightDarkColourSwitch ? greenColour : creamColour;

                // ensures first tile on the next row is the same colour last tile in row
                if (count % 8 != 0) lightDarkColourSwitch = !lightDarkColourSwitch;
                count += 1;
            }
        }


        private static (float X, float Y)[,] CreateBoardPositions()
        {
            var board = new (float X, float Y)[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                board[i, j] = (i + 0.5f, j + 0.5f);
            return board;
        }
    }
}