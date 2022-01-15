using Controllers.Interfaces;
using Models.Services.AI.Implementations;
using Models.Services.Game.Interfaces;
using Models.Utils.ExtensionMethods.PieceTypeExt;
using Zenject;

namespace Controllers.Commands
{
    public class AiMoveCommand: ICommand
    {
        private readonly IGameStateController _gameStateController;
        private readonly AiMoveExecutor _aiMoveExecutor;
        

        public AiMoveCommand(IGameStateController gameStateController, AiMoveExecutor aiMoveExecutor)
        {
            _gameStateController = gameStateController;
            _aiMoveExecutor = aiMoveExecutor;
        }

        public void Execute()
        {
            _aiMoveExecutor.MakeMove(_gameStateController, _gameStateController.Turn.NextTurn());
        }

        public void Undo()
        {
            _gameStateController.RevertGameState();
            _gameStateController.RetainBoardState();
        }

        public bool IsValid(bool peak) => true;

        public class Factory : PlaceholderFactory<AiMoveCommand>
        {
        }
    }
}