using UnityEngine;
using Zenject;

public class DragAndDropCommand : ICommand
{
    private static MoveDataFactory _moveDataFactory;
    private static IPieceMover _pieceMover;

    private IMoveData _moveData;
    private GameObject _piece;
    private IBoardPosition _destination;

    public DragAndDropCommand(
        GameObject piece,
        IBoardPosition destination,
        MoveDataFactory moveDataFactory,
        IPieceMover pieceMover
        ) 
    {
        _moveDataFactory = moveDataFactory;
        _pieceMover = pieceMover;
        _piece = piece;
        _destination = destination;
        _moveData = _moveDataFactory.CreateMoveData(piece, destination);
    }

    public void Execute()
    {
        _pieceMover.Move(_piece, _destination);
    }

    public bool IsValid()
    {
        return true;
    }

    public void Undo()
    {
        _pieceMover.UndoMove(_moveData);
    }

    public class Factory : PlaceholderFactory<GameObject, IBoardPosition, DragAndDropCommand> { }
}