using System.Collections.Immutable;
using Models.State.Board;

namespace Models.Services.Game.Interfaces
{
    public interface IGameOverEval
    {
        bool CheckMate(bool check, ImmutableDictionary<Position, ImmutableHashSet<Position>> possiblePieceMoves);
    }
}