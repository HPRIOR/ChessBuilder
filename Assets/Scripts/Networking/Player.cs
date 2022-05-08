using Controllers.Factories;
using Controllers.Interfaces;
using Mirror;
using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Networking
{
    public class Player : NetworkBehaviour
    {
        private BuildCommandFactory _buildCommandFactory;
        private ICommandInvoker _commandInvoker;
        private GameContext _context;
        private IGameStateController _gameStateController;
        private MoveCommandFactory _moveCommandFactory;
        private NetworkEvents _networkEvents;

        public void Start()
        {
            if (isLocalPlayer) _networkEvents.InvokeEvent(NetworkEvent.PlayerPrefabReady);
        }

        /*
         * ZenAutoInjection used so that prefab instantiation by Mirror will inject dependencies
         */
        [Inject]
        public void Construct(
            IGameStateController gameStateController,
            ICommandInvoker commandInvoker,
            MoveCommandFactory moveCommandFactory,
            BuildCommandFactory buildCommandFactory,
            NetworkEvents networkEvents,
            GameContext context
        )
        {
            _gameStateController = gameStateController;
            _commandInvoker = commandInvoker;
            _moveCommandFactory = moveCommandFactory;
            _buildCommandFactory = buildCommandFactory;
            _networkEvents = networkEvents;
            _context = context;
        }

        [ClientRpc]
        public void SetPlayerContext(PieceColour pieceColour)
        {
            if (isLocalPlayer)
            {
                _context.PlayerColour = pieceColour;
                _networkEvents.InvokeEvent(NetworkEvent.ContextReady);
            }
        }

        [ClientRpc]
        public void SetGameReady()
        {
            if (isLocalPlayer)
                _networkEvents.InvokeEvent(NetworkEvent.GameReady);
        }

        [Command]
        public void TryAddCommand(Position start, Position destination)
        {
            if (_gameStateController.IsValidMove(start, destination))
                AddCommand(start, destination);
            else
                RetainBoardState();
        }

        [Command]
        public void TryAddCommand(Position start, PieceType pieceType)
        {
            if (_gameStateController.IsValidMove(start, pieceType))
                AddCommand(start, pieceType);
            else
                RetainBoardState();
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
    }
}