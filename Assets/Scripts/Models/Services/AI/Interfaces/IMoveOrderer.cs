using System;
using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.Services.AI.Interfaces
{
    public interface IMoveOrderer
    {
        void OrderMoves(IEnumerable<Action<PieceColour>> moves);
    }
}