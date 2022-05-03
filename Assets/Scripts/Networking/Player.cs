using Controllers.Factories;
using Controllers.Interfaces;
using Mirror;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using Zenject;

namespace Networking
{
    public class Player : NetworkBehaviour
    {
        private IGameStateController _gameStateController;
        private ICommandInvoker _commandInvoker;
        private MoveCommandFactory _moveCommandFactory;
        private BuildCommandFactory _buildCommandFactory;

        /*
         * ZenAutoInjection used so that normal instantiation by Mirror will inject dependencies
         */
        [Inject]
        public void Construct(IGameStateController gameStateController, ICommandInvoker commandInvoker,
            MoveCommandFactory moveCommandFactory, BuildCommandFactory buildCommandFactory)
        {
            _gameStateController = gameStateController;
            _commandInvoker = commandInvoker;
            _moveCommandFactory = moveCommandFactory;
            _buildCommandFactory = buildCommandFactory;
        }

        [Command]
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

        [Command]
        public void TryAddCommand(Position start, PieceType pieceType)
        {
            if (_gameStateController.IsValidMove(start, pieceType))
            {
                AddCommand(start, pieceType);
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
        private void AddCommand(Position start, PieceType pieceType)
        {
            _commandInvoker.AddCommand(_buildCommandFactory.Create(start, pieceType));
        }

        [ClientRpc]
        private void RetainBoardState()
        {
            _gameStateController.RetainBoardState();
        }


        public class Factory : PlaceholderFactory<Player>
        {
        }
    }
}