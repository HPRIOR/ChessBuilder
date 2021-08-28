using System.Collections.Generic;
using Models.Services.AI.Implementations;
using Models.State.Board;

namespace Models.Services.AI.Interfaces
{
    public interface IMoveOrderer
    {
        void OrderMoves(IEnumerable<AiMove> moves, BoardState boardState);
    }
}