using System;
using Mirror;
using Models.Services.Game.Implementations;
using Models.State.Board;
using Models.State.GameState;
using UnityEngine;

namespace Networking
{
    public class NetworkState : NetworkBehaviour
    {
        private readonly SyncList<int> _syncList = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        [SyncVar(hook = "GameStateChangeHook")]
        private GameState _gameState;

        private event Action<GameState> GameStateChangeEvent;

        public void GameStateChangeHook(GameState old, GameState _new)
        {
            GameStateChangeEvent?.Invoke(_new);
        }

        public override void OnStartServer()
        {
            _syncList.Callback += OnSyncListUpdate;
        }


        public void AddCallBack(Action<GameState> gameStateChangeCallBack)
        {
            GameStateChangeEvent += gameStateChangeCallBack;
        }

        void OnSyncListUpdate(SyncList<int>.Operation op, int index, int oldItem, int newItem)
        {
            Debug.Log($"new game state is {newItem}");
        }

        public void UpdateNetworkState(GameState gameState)
        {
            _gameState = gameState;
        }
    }
}