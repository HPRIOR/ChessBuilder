using System;
using Models.State.Board;
using Models.State.GameState;

namespace Models.Services.Game.Interfaces
{
    public interface ITurnEventInvoker
    {
        event Action<BoardState, BoardState> BoardStateChangeEvent;
        event Action<GameState, GameState> GameStateChangeEvent;
    }
}