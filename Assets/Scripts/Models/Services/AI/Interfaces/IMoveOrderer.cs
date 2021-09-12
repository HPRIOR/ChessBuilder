using System.Collections.Generic;
using Models.Services.AI.Implementations;
using Models.State.Board;

namespace Models.Services.AI.Interfaces
{
    public interface IMoveOrderer
    {
        IEnumerable<AiMove> OrderMoves(IEnumerable<AiMove> moves, BoardState boardState);
    }
}