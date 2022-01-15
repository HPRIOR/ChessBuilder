using System;
using System.Collections.Generic;
using Controllers.Interfaces;
using Models.Services.AI.Implementations;

namespace Controllers.Commands
{
    public class CommandInvoker : ICommandInvoker
    {
        private readonly Stack<ICommand> _commandHistoryBuffer;
        public CommandInvoker()
        {
            _commandHistoryBuffer = new Stack<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            if (command.IsValid())
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