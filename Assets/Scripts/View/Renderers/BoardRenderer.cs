using Models.State.Board;
using UnityEngine;
using View.Interfaces;
using View.Prefab.Factories;

namespace View.Renderers
{
    public class BoardRenderer : IRenderer
    {
        private readonly TileFactory _tileFactory;

        public BoardRenderer(TileFactory tileFactory)
        {
            _tileFactory = tileFactory;
        }

        public void Render()
        {
            var board = CreateBoardPositions();
            var boardParent = new GameObject("BoardRender");
            var lightDarkColourSwitch = true;
            var greenColour = new Color32(118, 150, 86, 255);
            var creamColour = new Color32(238, 238, 210, 255);
            var count = 1;
            foreach (var (x, y) in board)
            {
                _tileFactory.CreateTile(new Position(x, y), boardParent,
                    lightDarkColourSwitch ? greenColour : creamColour);

                if (count % 8 != 0) lightDarkColourSwitch = !lightDarkColourSwitch;
                count += 1;
            }
        }


        private static (int X, int Y)[,] CreateBoardPositions()
        {
            var board = new (int X, int Y)[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                board[i, j] = (i, j);
            return board;
        }
    }
}