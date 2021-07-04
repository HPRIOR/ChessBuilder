using Controllers.Interfaces;
using Models.Services.Build.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Controllers.Commands
{
    public class BuildCommand : ICommand
    {
        private static IBuilder _builder;
        private static IBuildValidator _buildValidator;
        private readonly Position _at;
        private readonly IGameState _gameState;
        private readonly PieceType _piece;
        private readonly BoardState _stateTransitionedFrom;

        public BuildCommand(
            Position at,
            PieceType piece,
            IBuilder builder,
            IBuildValidator buildValidator,
            IGameState gameState)
        {
            _at = at;
            _piece = piece;
            _builder = builder;
            _buildValidator = buildValidator;
            _gameState = gameState;
            _stateTransitionedFrom = _gameState.CurrentBoardState;
        }

        public void Execute()
        {
            var newBoardState = _builder.GenerateNewBoardState(_gameState.CurrentBoardState, _at, _piece);
            _gameState.UpdateBoardState(newBoardState);
        }

        public void Undo()
        {
            _gameState.UpdateBoardState(_stateTransitionedFrom);
        }

        public bool IsValid()
        {
            if (_buildValidator.ValidateBuild(_gameState.PossibleBuildMoves, _at, _piece))
                return true;

            _gameState.RetainBoardState();
            return false;
        }


        public class Factory : PlaceholderFactory<Position, PieceType, BuildCommand>
        {
        }
    }
}