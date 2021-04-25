using System;
using Models.State.Interfaces;

namespace Game.Interfaces
{
    public interface ITurnEventInvoker
    {
        event Action<IBoardState, IBoardState> GameStateChangeEvent;
    }
}
