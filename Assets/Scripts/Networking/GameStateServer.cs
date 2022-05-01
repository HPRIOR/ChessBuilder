using System;
using Mirror;
using Models.Services.Game.Interfaces;
using Models.State.GameState;
using UnityEngine;
using Zenject;

namespace Networking
{
    /// <summary>
    /// Responsible for syncing GameState to clients
    /// </summary>
    public class GameStateServer: NetworkBehaviour
    {
        [Inject]
        void Construct(ITurnEventInvoker turnEventInvoker)
        {
            // this might need to be done in the awake function
            turnEventInvoker.GameStateChangeEvent += UpdateClientGameState;
        }

        void UpdateClientGameState(GameState previousGameState, GameState newGameState)
        {
            // communicate GameState changes with clients 
        }
    }
}