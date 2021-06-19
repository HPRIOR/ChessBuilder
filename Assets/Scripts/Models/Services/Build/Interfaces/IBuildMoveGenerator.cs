using System.Collections.Generic;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;

namespace Models.Services.Build.Interfaces
{
    public interface IBuildMoveGenerator
    {
        IEnumerable<Position> GetPossibleBuildMoves(BoardState currentBoardState, Turn turn);
    }
}