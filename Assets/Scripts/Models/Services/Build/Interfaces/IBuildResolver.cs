using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildResolver
    {
        public void ResolveBuilds(BoardState boardState, PieceColour turn);
    }
}