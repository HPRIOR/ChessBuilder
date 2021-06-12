using System;
using System.Collections.Generic;
using Controllers.Interfaces;

namespace Controllers.Commands
{
    public class CommandInvoker : ICommandInvoker
    {
        private readonly Stack<ICommand> _commandBuffer;

        public CommandInvoker()
        {
            _commandBuffer = new Stack<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            if (command.IsValid())
            {
                command.Execute();
                _commandBuffer.Push(command);
            }
        }

        public void RollBackCommand()
        {
            if (_commandBuffer.Count > 0) _commandBuffer.Pop().Undo();
        }

        public void UndoCommand()
        {
            throw new NotImplementedException();
        }
    }
}