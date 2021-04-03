using System;
using System.Collections.Generic;

public class GameState : IGameState
{
    private IPossibleMovesGenerator _possibleMovesGenerator;

    public GameState(IPossibleMovesGenerator possibleMovesGenerator)
    {
        _possibleMovesGenerator = possibleMovesGenerator;
    }

    public IBoardState currentBoardState { get; private set; }

    public IDictionary<IBoardPosition, HashSet<IBoardPosition>> PossibleBoardMoves { get; private set; }

    public void UpdateGameState(IBoardState newState)
    {
        var previousState = currentBoardState;
        currentBoardState = newState;

        PossibleBoardMoves = _possibleMovesGenerator.GeneratePossibleMoves(currentBoardState);
        GameStateChangeEvent?.Invoke(previousState, currentBoardState);
    }

    public event Action<IBoardState, IBoardState> GameStateChangeEvent;
}