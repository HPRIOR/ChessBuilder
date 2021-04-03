using System;

public interface IGameState
{
    IBoardState currentBoardState { get; }
    void UpdateGameState(IBoardState newState);
    event Action<IBoardState> GameStateChangeEvent;


}