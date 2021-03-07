using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    public ICommand currentCommand { get; set; }
    private IList<ICommand> commandBuffer;

    private void Start()
    {
        commandBuffer = new List<ICommand>();
    }
    private void Update()
    {
        if (currentCommand != null)
        {
            currentCommand.Execute();
            commandBuffer.Add(currentCommand);
            currentCommand = null;
        }
    }

}
