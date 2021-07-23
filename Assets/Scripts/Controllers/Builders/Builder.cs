using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Controllers.Builders
{
    public class Builder : IBuilder
    {
        public BoardState GenerateNewBoardState(BoardState previousBoardState, Position buildPosition, PieceType piece)
        {
            var newActiveBuilds = new HashSet<Position>(previousBoardState.ActiveBuilds) {buildPosition};
            var newActivePieces = new HashSet<Position>(previousBoardState.ActivePieces);

            var newBoardState = previousBoardState.CloneWithDecrementBuildState(newActivePieces, newActiveBuilds);

            var buildTile = newBoardState.Board[buildPosition.X, buildPosition.Y];
            buildTile.BuildTileState = new BuildTileState(piece);

            return newBoardState;
        }
    }
}