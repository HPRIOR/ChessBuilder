using UnityEngine;

public class MoveDataFactory
{
    private readonly MoveData.Factory _moveDataFactory;

    public MoveDataFactory(MoveData.Factory moveDataFactory)
    {
        _moveDataFactory = moveDataFactory;
    }

    public IMoveData CreateMoveData(GameObject piece, IBoardPosition destination) =>
        _moveDataFactory.Create(piece, destination);
}