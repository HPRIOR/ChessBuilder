using Controllers.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Controllers.Commands
{
    public class BuildCommand : ICommand
    {
        private static IBuildValidator _buildValidator;
        private readonly Position _at;
        private readonly IGameStateController _gameStateController;
        private readonly PieceType _piece;

        public BuildCommand(Position at,
            PieceType piece,
            IBuildValidator buildValidator,
            IGameStateController gameStateController)
        {
            _at = at;
            _piece = piece;
            _buildValidator = buildValidator;
            _gameStateController = gameStateController;
        }

        public void Execute()
        {
            _gameStateController.UpdateGameState(_at, _piece);
        }

        public void Undo()
        {
            _gameStateController.RevertGameState();
            _gameStateController.RetainBoardState();
        }

        public bool IsValid(bool peak)
        {
            if (_buildValidator.ValidateBuild(_gameStateController.CurrentGameState.PossibleBuildMoves, _at, _piece))
                return true;

            _gameStateController.RetainBoardState();
            return false;
        }


        public class Factory : PlaceholderFactory<Position, PieceType, BuildCommand>
        {
        }
    }
}