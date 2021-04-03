using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameState: IGameState
{
    IPossibleMovesGenerator _possibleMovesGenerator;
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

    /*
     * Communcates change in game state. Used for various things:
     *  rendering pieces 
     *  calculating possible moves at the end of a turn                            
     */
    public event Action<IBoardState, IBoardState> GameStateChangeEvent;


}
