using System;
using Controllers.Commands;
using Controllers.Factories;
using Controllers.Interfaces;
using Mirror;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;
using UnityEngine;
using Zenject;

namespace Networking
{
    public class MoveServer : NetworkBehaviour
    {
        private IGameStateController _gameStateController;
        private ICommandInvoker _commandInvoker;
        private MoveCommandFactory _moveCommandFactory;

        [Inject]
        public void Construct(IGameStateController gameStateController, ICommandInvoker commandInvoker,
            MoveCommandFactory moveCommandFactory)
        {
            _gameStateController = gameStateController;
            _commandInvoker = commandInvoker;
            _moveCommandFactory = moveCommandFactory;
        }

        public void TryAddCommand(Position start, Position destination)
        {
            if (_gameStateController.IsValidMove(start, destination))
            {
                AddCommand(start, destination);
            }
            else
            {
                RetainBoardState();
            }
        }

        [ClientRpc]
        private void AddCommand(Position start, Position destination)
        {
            _commandInvoker.AddCommand(_moveCommandFactory.Create(start, destination));
        }

        [ClientRpc]
        private void RetainBoardState()
        {
            _gameStateController.RetainBoardState();
        }
    }
}