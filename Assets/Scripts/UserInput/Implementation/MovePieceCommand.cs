using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieceCommand : ICommand
{

    public MovePieceCommand(GameObject piece, IBoardPosition destination)
    {

    }
    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public bool IsValid()
    {
        throw new System.NotImplementedException();
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
