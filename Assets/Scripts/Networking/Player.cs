using System.Collections;
using Controllers.Factories;
using Controllers.Interfaces;
using Mirror;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using Zenject;

namespace Networking
{
    public class Player : NetworkBehaviour
    {
        private ICommandInvoker _commandInvoker;
        private MoveCommandFactory _moveCommandFactory;
        private BuildCommandFactory _buildCommandFactory;

        [Inject]
        public void Construct(ICommandInvoker commandInvoker, MoveCommandFactory moveCommandFactory,
            BuildCommandFactory buildCommandFactory)
        {
            _commandInvoker = commandInvoker;
            _moveCommandFactory = moveCommandFactory;
            _buildCommandFactory = buildCommandFactory;
        }

        [Command]
        public void AddCommand(Position start, Position destination)
        {
            _commandInvoker.AddCommand(_moveCommandFactory.Create(start, destination));
        }

        [Command]
        public void AddCommand(Position pos, PieceType pieceType)
        {
            _commandInvoker.AddCommand(_buildCommandFactory.Create(pos, pieceType));
        }


        public class Factory : PlaceholderFactory<Player>
        {
        }
    }
}