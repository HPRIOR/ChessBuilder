using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MovePieceCommandFactory
{
    private MovePieceCommand.Factory _movePieceCommandFactory;
    
    public MovePieceCommandFactory(MovePieceCommand.Factory movePieceCommandFactory)
    {
        _movePieceCommandFactory = movePieceCommandFactory;
    }

    public MovePieceCommand Create(GameObject piece, IBoardPosition destination)
    {
        return _movePieceCommandFactory.Create(piece, destination);
    }
}
