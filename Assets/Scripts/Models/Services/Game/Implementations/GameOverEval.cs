using System.Collections.Generic;
using System.Linq;
using Models.Services.Game.Interfaces;
using Models.State.Board;

namespace Models.Services.Game.Implementations
{
    public class GameOverEval : IGameOverEval
    {
        public bool CheckMate(bool check,
            IDictionary<Position, List<Position>> possiblePieceMoves) =>
            check && !possiblePieceMoves.Any(moves => moves.Value.Count > 0);
    }
}