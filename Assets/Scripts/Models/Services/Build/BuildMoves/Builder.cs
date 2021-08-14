using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.Services.Build.BuildMoves
{
    public class Builder : IBuilder
    {
        public BoardState GenerateNewBoardState(BoardState previousBoardState, Position buildPosition, PieceType piece)
        {
            // add build positions to active pieces 
            var newActiveBuilds = new HashSet<Position>(previousBoardState.ActiveBuilds) { buildPosition };

            // do nothing to active pieces
            var newActivePieces = new HashSet<Position>(previousBoardState.ActivePieces);

            // decrement builds 
            var newBoardState = previousBoardState.CloneWithDecrementBuildState(newActivePieces, newActiveBuilds);

            // modify board state 
            var buildTile = newBoardState.Board[buildPosition.X, buildPosition.Y];
            buildTile.BuildTileState = new BuildTileState(piece);

            // void return
            return newBoardState;
        }
    }
}