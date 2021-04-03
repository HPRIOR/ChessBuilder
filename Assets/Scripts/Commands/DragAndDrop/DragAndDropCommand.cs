using UnityEngine;
using Zenject;

public class DragAndDropCommand : ICommand
{
    private static IPieceMover _pieceMover;
    private static IMoveValidator _moveValidator;
    private GameObject _piece;
    private IBoardPosition _destination;

    public DragAndDropCommand(
        GameObject piece,
        IBoardPosition destination,
        IPieceMover pieceMover,
        IMoveValidator moveValidator
        )
    {

        _piece = piece;
        _destination = destination;

        _moveValidator = moveValidator;
        _pieceMover = pieceMover;
    }

    public void Execute()
    {
        //_pieceMover.Move(_piece, _destination);
    }

    public bool IsValid()
    {
        //if (_moveValidator.ValidateMove(_piece, _destination))
        //    return true;
        //else
        //{
        //    _piece.transform.position = _piece.GetComponent<Piece>().BoardPosition.Vector;
        //    return false;
        //}
        return true;
    }

    public void Undo()
    {
    }

    public class Factory : PlaceholderFactory<GameObject, IBoardPosition, DragAndDropCommand> { }
}