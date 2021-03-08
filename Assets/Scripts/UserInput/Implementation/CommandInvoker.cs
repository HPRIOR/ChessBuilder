using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{

    /*
     * player 1/2 logic could be implemented here
     * instead of the piece mover switching between moves two buffers are kept and commands are invoked from it in turn
     */
    public ICommand currentCommand { get; set; }
    private IList<ICommand> commandBuffer;

    private void Start()
    {
        commandBuffer = new List<ICommand>();
    }
    private void Update()
    {
        if (currentCommand != null && currentCommand.IsValid())
        {
            currentCommand.Execute();
            commandBuffer.Add(currentCommand);
            currentCommand = null;
        }
    }

}
