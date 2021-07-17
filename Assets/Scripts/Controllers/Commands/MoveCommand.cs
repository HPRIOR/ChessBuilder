using Controllers.Interfaces;
using Models.Services.Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Zenject;

namespace Controllers.Commands
{
    public class MoveCommand : ICommand
    {
        private static IPieceMover _pieceMover;
        private static IMoveValidator _moveValidator;
        private readonly Position _destination;
        private readonly Position _from;
        private readonly IGameStateController _gameStateController;
        private readonly BoardState _stateTransitionedFrom;


        public MoveCommand(
            Position from,
            Position destination,
            IPieceMover pieceMover,
            IMoveValidator moveValidator,
            IGameStateController gameStateController
        )
        {
            _gameStateController = gameStateController;
            _stateTransitionedFrom = _gameStateController.CurrentGameState.BoardState;

            _from = from;
            _destination = destination;

            _moveValidator = moveValidator;
            _pieceMover = pieceMover;
        }

        public void Execute()
        {
            var newBoardState =
                _pieceMover.GenerateNewBoardState(_gameStateController.CurrentGameState.BoardState, _from,
                    _destination);
            _gameStateController.UpdateGameState(newBoardState);
        }

        public bool IsValid()
        {
            if (_moveValidator.ValidateMove(_gameStateController.CurrentGameState.PossiblePieceMoves, _from,
                _destination))
                return true;


            _gameStateController.RetainBoardState();
            return false;
        }

        public void Undo()
        {
            _gameStateController.UpdateGameState(_stateTransitionedFrom);
        }

        public class Factory : PlaceholderFactory<Position, Position, MoveCommand>
        {
        }
    }
}