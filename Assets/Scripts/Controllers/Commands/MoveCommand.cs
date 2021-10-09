using Controllers.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Zenject;

namespace Controllers.Commands
{
    public class MoveCommand : ICommand
    {
        private static IMoveValidator _moveValidator;
        private readonly Position _destination;
        private readonly Position _from;
        private readonly IGameStateController _gameStateController;


        public MoveCommand(Position from,
            Position destination,
            IMoveValidator moveValidator,
            IGameStateController gameStateController)
        {
            _gameStateController = gameStateController;

            _from = from;
            _destination = destination;

            _moveValidator = moveValidator;
        }

        public void Execute()
        {
            _gameStateController.UpdateGameState(_from, _destination);
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
            _gameStateController.RevertGameState();
            _gameStateController.RetainBoardState();
        }

        public class Factory : PlaceholderFactory<Position, Position, MoveCommand>
        {
        }
    }
}