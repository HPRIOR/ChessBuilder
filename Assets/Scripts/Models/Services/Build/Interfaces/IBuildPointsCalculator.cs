using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildPointsCalculator
    {
        PlayerState CalculateBuildPoints(PieceColour pieceColour, BoardState boardState, int maxPoints);
    }
}