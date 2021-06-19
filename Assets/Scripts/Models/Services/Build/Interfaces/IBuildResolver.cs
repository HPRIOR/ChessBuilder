using Models.State.Board;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildResolver
    {
        public void ResolveBuild(BoardState boardState);
    }
}