﻿using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;

namespace Models.Services.Build.Utils
{
    public class BuildResolver : IBuildResolver
    {
        public void ResolveBuilds(BoardState boardState, PieceColour turn)
        {
            foreach (var tile in boardState.Board)
            {
                var canBuild = tile.BuildTileState.Turns == 0 &&
                               tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                               tile.CurrentPiece.Type == PieceType.NullPiece &&
                               tile.BuildTileState.BuildingPiece.Colour() == turn;
                if (canBuild)
                {
                    // Account for new active piece in active builds and pieces
                    boardState.ActiveBuilds.Remove(tile.Position);
                    boardState.ActivePieces.Add(tile.Position);

                    tile.CurrentPiece = new Piece(tile.BuildTileState.BuildingPiece);
                    tile.BuildTileState = new BuildTileState(); // reset build state
                }
            }
        }
    }
}