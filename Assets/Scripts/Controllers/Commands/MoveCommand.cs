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


        // TODO: behavior of this may need to be change -> board state is referenced and not saved per command
        // whether or not it is valid will be determined by the current state of the board
        // for AI move there may be many states existing concurrently 
        // undo behaviour may change too 
        public MoveCommand(
            Position from,
            Position destination,
            IPieceMover pieceMover,
            IMoveValidator moveValidator,
            IGameStateController gameStateController
        )
        {
            _gameStateController = gameStateController;
            _stateTransitionedFrom = _gameStateController.CurrentBoardState;

            _from = from;
            _destination = destination;

            _moveValidator = moveValidator;
            _pieceMover = pieceMover;
        }

        public void Execute()
        {
            var newBoardState =
                _pieceMover.GenerateNewBoardState(_gameStateController.CurrentBoardState, _from, _destination);
            _gameStateController.UpdateBoardState(newBoardState);
        }

        public bool IsValid()
        {
            if (_moveValidator.ValidateMove(_gameStateController.PossiblePieceMoves, _from, _destination))
                return true;


            _gameStateController.RetainBoardState();
            return false;
        }

        public void Undo()
        {
            _gameStateController.UpdateBoardState(_stateTransitionedFrom);
        }

        public class Factory : PlaceholderFactory<Position, Position, MoveCommand>
        {
        }
    }
}