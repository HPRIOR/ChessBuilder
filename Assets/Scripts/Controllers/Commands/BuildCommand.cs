﻿using Controllers.Interfaces;
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
        private readonly IGameStateController _gameStateController;
        private readonly PieceType _piece;
        private readonly BoardState _stateTransitionedFrom;

        public BuildCommand(
            Position at,
            PieceType piece,
            IBuilder builder,
            IBuildValidator buildValidator,
            IGameStateController gameStateController)
        {
            _at = at;
            _piece = piece;
            _builder = builder;
            _buildValidator = buildValidator;
            _gameStateController = gameStateController;
            _stateTransitionedFrom = _gameStateController.CurrentGameState.BoardState;
        }

        public void Execute()
        {
            var newBoardState =
                _builder.GenerateNewBoardState(_gameStateController.CurrentGameState.BoardState, _at, _piece);
            _gameStateController.UpdateGameState(newBoardState);
        }

        public void Undo()
        {
            _gameStateController.UpdateGameState(_stateTransitionedFrom);
        }

        public bool IsValid()
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