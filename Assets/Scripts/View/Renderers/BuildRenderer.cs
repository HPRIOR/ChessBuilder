﻿using Models.State.Board;
using Models.State.PieceState;
using View.Interfaces;
using View.Prefab.Factories;
using View.Utils;

namespace View.Renderers
{
    public class BuildRenderer : IStateChangeRenderer
    {
        private readonly BuildingPieceFactory _buildingPieceFactory;

        public BuildRenderer(BuildingPieceFactory buildingPieceFactory)
        {
            _buildingPieceFactory = buildingPieceFactory;
        }

        public void Render(BoardState previousState, BoardState newState)
        {
            GameObjectDestroyer.DestroyChildrenOfObjectWith("BuildingPieces");
            var board = newState.Board;
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                var tile = board[i][j];
                var isBuilding = tile.BuildTileState.BuildingPiece != PieceType.NullPiece;
                if (isBuilding)
                    _buildingPieceFactory.Create(tile.BuildTileState, tile.Position);
            }
        }
    }
}