using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildMoveGenerator
    {
        State.BuildState.BuildMoves GetPossibleBuildMoves(BoardState boardState, PieceColour turn,
            PlayerState playerState);
    }
}