using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Controllers.Builders
{
    public class Builder : IBuilder
    {
        private readonly IBuildResolver _buildResolver;

        public Builder(IBuildResolver buildResolver)
        {
            _buildResolver = buildResolver;
        }

        public BoardState GenerateNewBoardState(BoardState previousBoardState, Position buildPosition, PieceType piece)
        {
            var newBoardState = previousBoardState.CloneWithDecrementBuildState();

            var buildTile = newBoardState.Board[buildPosition.X, buildPosition.Y];
            buildTile.BuildState = new BuildState(piece);

            // _buildResolver.ResolveBuild(newBoardState, piece.Colour());

            return newBoardState;
        }
    }
}