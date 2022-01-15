using System;
using System.Collections.Generic;
using Controllers.Interfaces;
using Models.Services.AI.Implementations;
using Models.Services.Game.Interfaces;

namespace Controllers.Commands
{
    public class CommandInvoker : ICommandInvoker
    {
        private readonly IGameStateController _gameStateController;
        private readonly Stack<ICommand> _commandHistoryBuffer;
        public CommandInvoker(IGameStateController gameStateController)
        {
            _gameStateController = gameStateController;
            _commandHistoryBuffer = new Stack<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            if (command.IsValid(peak: false))
            {
                command.Execute();
                _commandHistoryBuffer.Push(command);
            }
        }

        public void RollBackCommand()
        {
            if (_commandHistoryBuffer.Count > 0) _commandHistoryBuffer.Pop().Undo();
        }

        public void UndoCommand()
        {
            throw new NotImplementedException();
        }
    }
}