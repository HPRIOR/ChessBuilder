using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildMoveGenerator
    {
        IDictionary<Position, HashSet<PieceType>> GetPossibleBuildMoves(BoardState boardState, PieceColour turn);
    }
}