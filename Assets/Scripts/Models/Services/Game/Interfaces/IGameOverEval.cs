using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Game.Interfaces
{
    public interface IGameOverEval
    {
        bool CheckMate(bool check, IDictionary<Position, HashSet<Position>> possiblePieceMoves);
    }
}