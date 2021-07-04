using System;
using Models.State.Board;

namespace Models.Services.Game.Interfaces
{
    public interface ITurnEventInvoker
    {
        event Action<BoardState, BoardState> GameStateChangeEvent;
    }
}