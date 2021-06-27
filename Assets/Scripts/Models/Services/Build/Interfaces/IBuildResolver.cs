using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildResolver
    {
        public void ResolveBuild(BoardState boardState, PieceColour turn);
    }
}