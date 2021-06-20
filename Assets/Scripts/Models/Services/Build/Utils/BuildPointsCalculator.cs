using System;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Build.Utils
{
    public class BuildPointsCalculator : IBuildPointsCalculator
    {
        public PlayerState CalculateBuildPoints(PieceColour pieceColour, BoardState boardState,
            int maxPoints) => throw new NotImplementedException();
    }
}