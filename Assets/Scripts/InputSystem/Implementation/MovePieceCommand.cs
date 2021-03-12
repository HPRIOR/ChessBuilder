using UnityEngine;

public class MovePieceCommand : ICommand
{
    private static GameController _gameController
        = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    private static IPieceMover _pieceMover = new PieceMover();

    private readonly IMoveData _moveData;
    private readonly ITile _toTile;
    private readonly ITile _fromTile;
    private readonly GameObject _piece;
    private readonly IBoardPosition _destination;

    public MovePieceCommand(GameObject piece, IBoardPosition destination)
    {
        _fromTile = _gameController.GetTileAt(piece.GetComponent<Piece>().BoardPosition);
        _toTile = _gameController.GetTileAt(destination);
        _piece = piece;
        _destination = destination;
        _moveData = new MoveData(
            piece,
            destination
        );
    }

    public void Execute()
    {
        _pieceMover.Move(_piece, _destination);
    }

    public bool IsValid()
    {
        if (_toTile == _fromTile)
        {
            _moveData.MovedPiece.transform.position = _moveData.InitialBoardPosition.Position;
            return false;
        }
        return true;
    }

    public void Undo()
    {
        _pieceMover.UndoMove(_moveData);
    }
}