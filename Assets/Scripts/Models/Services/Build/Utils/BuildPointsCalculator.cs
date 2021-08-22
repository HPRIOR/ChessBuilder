﻿using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.State.PlayerState;
using Models.Utils.ExtensionMethods.PieceType;

namespace Models.Services.Build.Utils
{
    public class BuildPointsCalculator : IBuildPointsCalculator
    {
        public PlayerState CalculateBuildPoints(PieceColour pieceColour, BoardState boardState,
            int maxPoints)
        {
            var result = 0;
            // TODO remove inefficient linq
            var activeTiles = new List<Tile>();
            var board = boardState.Board;
            var activeBuilds = boardState.ActiveBuilds;
            var activePieces = boardState.ActivePieces;

            foreach (var pos in activeBuilds) activeTiles.Add(board[pos.X, pos.Y]);

            foreach (var pos in activePieces) activeTiles.Add(board[pos.X, pos.Y]);
            foreach (var tile in activeTiles)
            {
                var pieceIsOfColourType = tile.CurrentPiece.Colour == pieceColour &&
                                          tile.CurrentPiece.Type != PieceType.NullPiece;
                if (pieceIsOfColourType)
                    result += BuildPoints.PieceCost[tile.CurrentPiece.Type];

                var pieceOfColourIsBeingBuilt = tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                                                tile.BuildTileState.BuildingPiece.Colour() == pieceColour;
                if (pieceOfColourIsBeingBuilt)
                    result += BuildPoints.PieceCost[tile.BuildTileState.BuildingPiece];
            }

            return new PlayerState(maxPoints - result);
        }
    }
}