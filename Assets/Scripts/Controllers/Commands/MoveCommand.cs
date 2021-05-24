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
        private readonly BoardPosition _destination;
        private readonly BoardPosition _from;
        private readonly IGameState _gameState;
        private readonly BoardState _stateTransitionedFrom;

        public MoveCommand(
            BoardPosition from,
            BoardPosition destination,
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
            _gameState.UpdateGameState(newBoardState, _destination);
        }

        public bool IsValid()
        {
            if (_from.Equals(_destination)) return false;
            if (_moveValidator.ValidateMove(_gameState.PossiblePieceMoves, _from, _destination))
                return true;

            // return to original state
            // this will update turn incorrectly
            _gameState.UpdateGameState(_stateTransitionedFrom, _from);
            return false;
        }

        public void Undo()
        {
            _gameState.UpdateGameState(_stateTransitionedFrom, _from);
        }

        public class Factory : PlaceholderFactory<BoardPosition, BoardPosition, MoveCommand>
        {
        }
    }
}