using UnityEngine;
using Zenject;

public class DragAndDropCommand : ICommand
{
    private static MoveDataFactory _moveDataFactory;
    private static IPieceMover _pieceMover;
    private static IMoveValidator _moveValidator;

    private IMoveData _moveData;
    private GameObject _piece;
    private IBoardPosition _destination;

    public DragAndDropCommand(
        GameObject piece,
        IBoardPosition destination,
        MoveDataFactory moveDataFactory,
        IPieceMover pieceMover,
        IMoveValidator moveValidator
        ) 
    {
        _moveDataFactory = moveDataFactory;
        _pieceMover = pieceMover;
        _piece = piece;
        _destination = destination;
        _moveValidator = moveValidator;
        _moveData = _moveDataFactory.CreateMoveData(piece, destination);
    }

    public void Execute()
    {
        _pieceMover.Move(_piece, _destination);
    }

    public bool IsValid()
    {
        if (_moveValidator.ValidateMove(_piece, _destination))
            return true;
        else
        {
            _piece.transform.position = _piece.GetComponent<Piece>().BoardPosition.Position;
            return false;
        }
    }

    public void Undo()
    {
        _pieceMover.UndoMove(_moveData);
    }

    public class Factory : PlaceholderFactory<GameObject, IBoardPosition, DragAndDropCommand> { }
}