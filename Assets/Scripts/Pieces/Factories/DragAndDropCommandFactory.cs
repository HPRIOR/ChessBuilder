using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DragAndDropCommandFactory
{
    private DragAndDropCommand.Factory _movePieceCommandFactory;
    
    public DragAndDropCommandFactory(DragAndDropCommand.Factory movePieceCommandFactory)
    {
        _movePieceCommandFactory = movePieceCommandFactory;
    }

    public DragAndDropCommand Create(GameObject piece, IBoardPosition destination)
    {
        return _movePieceCommandFactory.Create(piece, destination);
    }
}
