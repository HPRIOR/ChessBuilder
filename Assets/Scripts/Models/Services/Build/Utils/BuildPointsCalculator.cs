using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Build.Utils
{
    public class BuildPointsCalculator : IBuildPointsCalculator
    {
        // TODO inject max points 
        public PlayerState CalculateBuildPoints(PieceColour pieceColour, BoardState boardState,
            int maxPoints)
        {
            var result = 0;
            var activeBuilds = boardState.ActiveBuilds;
            var activePieces = boardState.ActivePieces;
            var activePositions = activeBuilds.Union(activePieces);
            
            foreach (var pos in activePositions)
            {
                ref var tile = ref boardState.GetTileAt(pos);
                var pieceIsOfColourType = tile.CurrentPiece.Colour == pieceColour &&
                                          tile.CurrentPiece.Type != PieceType.NullPiece;
                if (pieceIsOfColourType)
                    result += tile.CurrentPiece.Type.Value();

                var pieceOfColourIsBeingBuilt = tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                                                tile.BuildTileState.BuildingPiece.Colour() == pieceColour;
                if (pieceOfColourIsBeingBuilt)
                    result += tile.BuildTileState.BuildingPiece.Value();
            }

            return new PlayerState(maxPoints - result);
        }
    }
}