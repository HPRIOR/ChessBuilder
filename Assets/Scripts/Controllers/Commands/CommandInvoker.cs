using System;
using System.Collections.Generic;
using Controllers.Interfaces;
using ModestTree;
using UnityEngine;
using View.Interfaces;

namespace Controllers.Commands
{
    // TODO insure that each command is executed once per frame
    public class CommandInvoker : ICommandInvoker
    {
        private readonly Stack<ICommand> _commandHistoryBuffer;
        private readonly Queue<ICommand> _commandQueue;
        public CommandInvoker()
        {
            _commandQueue = new Queue<ICommand>();
            _commandHistoryBuffer = new Stack<ICommand>();
        }
        
        public void ExecuteCommand()
        {
            if (_commandQueue.IsEmpty())
                return;
            
            var command = _commandQueue.Dequeue();
            if (command.IsValid(peak: false))
            {
                command.Execute();
                _commandHistoryBuffer.Push(command);
            }
        }
        public void AddCommand(ICommand command)
        {
            _commandQueue.Enqueue(command);
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