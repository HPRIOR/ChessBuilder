using Controllers.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Zenject;

namespace Controllers.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly Position _destination;
        private readonly Position _from;
        private readonly IGameStateController _gameStateController;

        public MoveCommand(Position from,
            Position destination,
            IGameStateController gameStateController)
        {
            _gameStateController = gameStateController;

            _from = from;
            _destination = destination;
        }

        public void Execute()
        {
            _gameStateController.UpdateGameState(_from, _destination);
        }

        public bool IsValid(bool peak)
        {
            if (_gameStateController.IsValidMove(_from, _destination))
                return true;

            if (!peak) _gameStateController.RetainBoardState();
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