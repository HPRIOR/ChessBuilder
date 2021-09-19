using System.Collections.Generic;
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
            var activeTiles = new List<Tile>();
            var board = boardState.Board;
            var activeBuilds = boardState.ActiveBuilds;
            var activePieces = boardState.ActivePieces;

            foreach (var pos in activeBuilds) activeTiles.Add(board[pos.X][pos.Y]);
            foreach (var pos in activePieces) activeTiles.Add(board[pos.X][pos.Y]);
            foreach (var tile in activeTiles)
            {
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