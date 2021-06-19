using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildMoveGenerator
    {
        IEnumerable<Position> GetPossibleBuildMoves(BoardState boardState, PieceColour turn);
    }
}