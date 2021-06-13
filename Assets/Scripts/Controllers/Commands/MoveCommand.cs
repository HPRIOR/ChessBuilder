using Controllers.Interfaces;
using Game.Interfaces;
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
        private readonly IGameState _gameState;
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
            IGameState gameState
        )
        {
            _gameState = gameState;
            _stateTransitionedFrom = _gameState.CurrentBoardState;

            _from = from;
            _destination = destination;

            _moveValidator = moveValidator;
            _pieceMover = pieceMover;
        }

        public void Execute()
        {
            var newBoardState = _pieceMover.GenerateNewBoardState(_gameState.CurrentBoardState, _from, _destination);
            _gameState.UpdateBoardState(newBoardState);
        }

        public bool IsValid()
        {
            if (_moveValidator.ValidateMove(_gameState.PossiblePieceMoves, _from, _destination))
                return true;

            // return to original state
            // this will update turn incorrectly
            _gameState.RetainBoardState();
            return false;
        }

        public void Undo()
        {
            _gameState.UpdateBoardState(_stateTransitionedFrom);
        }

        public class Factory : PlaceholderFactory<Position, Position, MoveCommand>
        {
        }
    }
}