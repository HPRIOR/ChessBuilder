using System;
using System.Collections.Generic;

public interface IGameState
{
    IBoardState currentBoardState { get; }
    IDictionary<IBoardPosition, HashSet<IBoardPosition>> PossibleBoardMoves { get; }
    void UpdateGameState(IBoardState newState);
    event Action<IBoardState, IBoardState> GameStateChangeEvent;

}