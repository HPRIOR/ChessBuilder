using System.Collections.Generic;

public class CommandInvoker : ICommandInvoker
{
    private Stack<ICommand> commandBuffer;

    public CommandInvoker()
    {
        commandBuffer = new Stack<ICommand>();
    }

    public void AddCommand(ICommand command)
    {
        if (command.IsValid())
        {
            command.Execute();
            commandBuffer.Push(command);
        }
    }

    public void RollBackCommand()
    {
        if (commandBuffer.Count > 0) commandBuffer.Pop().Undo();
    }

    public void UndoCommand()
    {
        throw new System.NotImplementedException();
    }
}