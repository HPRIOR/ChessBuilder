using System.Collections.Generic;
using Models.Services.AI.Implementations;
using Models.State.GameState;

namespace Models.Services.AI.Interfaces
{
    public interface IAiPossibleMoveGenerator
    {
        IEnumerable<AiMove> GenerateMoves(GameState gameState);
    }
}