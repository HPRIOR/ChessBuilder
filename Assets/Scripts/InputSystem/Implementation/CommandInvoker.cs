using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{

    /*
     * player 1/2 logic could be implemented here
     * instead of the piece mover switching between moves two buffers are kept and commands are invoked from it in turn
     */
    private ICommand currentCommand;
    private Stack<ICommand> commandBuffer;

    private void Start()
    {
        commandBuffer = new Stack<ICommand>();
    }
    private void Update()
    {
        if (currentCommand != null && currentCommand.IsValid())
        {
            currentCommand.Execute();
            commandBuffer.Push(currentCommand);
            currentCommand = null;
        }
    }

    public void AddCommand(ICommand command) => currentCommand = command;

    public void RollBackCommand()
    {
        if (commandBuffer.Count > 0) commandBuffer.Pop().Undo();
    }
 

}
