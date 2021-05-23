using System;
using Models.State.Board;

namespace Game.Interfaces
{
    public interface ITurnEventInvoker
    {
        event Action<BoardState, BoardState> GameStateChangeEvent;
    }
}