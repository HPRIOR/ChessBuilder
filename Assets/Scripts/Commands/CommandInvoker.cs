using System.Collections.Generic;

public class CommandInvoker : ICommandInvoker
{
    /*
     * player 1/2 logic could be implemented here
     * instead of the piece mover switching between moves two buffers are kept and commands are invoked from it in turn
     */
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