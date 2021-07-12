using System.Collections.Immutable;
using System.Linq;
using Models.Services.Game.Interfaces;
using Models.State.Board;

namespace Models.Services.Game.Implementations
{
    public class GameOverEval : IGameOverEval
    {
        public bool CheckMate(bool check,
            ImmutableDictionary<Position, ImmutableHashSet<Position>> possiblePieceMoves) =>
            check && !possiblePieceMoves.Any(moves => moves.Value.Count > 0);
    }
}