﻿using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;

namespace Models.Services.Build.Utils
{
    public class BuildResolver : IBuildResolver
    {
        public void ResolveBuild(BoardState boardState, PieceColour turn)
        {
            foreach (var tile in boardState.Board)
            {
                var canBuild = tile.BuildState.Turns == 0 &&
                               tile.BuildState.BuildingPiece != PieceType.NullPiece &&
                               tile.CurrentPiece.Type == PieceType.NullPiece &&
                               tile.BuildState.BuildingPiece.Colour() == turn;
                if (canBuild)
                {
                    tile.CurrentPiece = new Piece(tile.BuildState.BuildingPiece);
                    tile.BuildState = new BuildState(); // reset build state
                }
            }
        }
    }
}