using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameState: IGameState
{
    public IBoardState currentBoardState { get; private set; }
    public void UpdateGameState(IBoardState newState)
    {
        currentBoardState = newState;
        GameStateChangeEvent?.Invoke(currentBoardState);
    }

    /*
     * Communcates change in game state. Used for various things:
     *  rendering pieces 
     *  calculating possible moves at the end of a turn                            
     */
    public event Action<IBoardState> GameStateChangeEvent;


}
