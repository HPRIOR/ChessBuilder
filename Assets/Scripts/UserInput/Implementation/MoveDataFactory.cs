using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveDataFactory
{
    private readonly MoveData.Factory _moveDataFactory;
    public MoveDataFactory(MoveData.Factory moveDataFactory)
    {
        _moveDataFactory = moveDataFactory;
    }

    public IMoveData CreateMoveData(GameObject piece, IBoardPosition destination)
    {
        return _moveDataFactory.Create(piece, destination);
    }
}
