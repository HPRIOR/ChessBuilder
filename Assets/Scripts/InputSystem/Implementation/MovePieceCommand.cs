using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MovePieceCommand : ICommand
{
    private static GameController _gameController 
        = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    private readonly IMoveData _moveData;
    private readonly ITile _toTile;
    private readonly ITile _fromTile;

    public MovePieceCommand(GameObject piece, IBoardPosition destination)
    {
        _fromTile = _gameController.GetTileAt(piece.GetComponent<Piece>().boardPosition);
        _toTile = _gameController.GetTileAt(destination);
        _moveData = new MoveData(
            piece,
            destination
        );
    }
    
    //[Inject]
    //private void Constructor(IPieceGenerator pieceGenerator)
    //{
    //    _pieceGenerator = pieceGenerator;
    //}

    public void Execute()
    {
        // handle GameObjects
        _moveData.MovedPiece.transform.position = _moveData.DestinationBoardPosition.Position;
        if (_moveData.DisplacedPiece != null)
            _moveData.DisplacedPiece.SetActive(false);

        // update boardinfo
        _toTile.CurrentPiece = _moveData.MovedPiece;
        _fromTile.CurrentPiece = null;

        // updatePieceInfo
        _moveData.MovedPieceComponent.boardPosition = _moveData.DestinationBoardPosition;
        _gameController.ChangeTurn();
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
        _moveData.MovedPiece.SetActive(true);
        _moveData.MovedPiece.transform.position = _moveData.InitialBoardPosition.Position;
        _moveData.MovedPieceComponent.boardPosition = _moveData.InitialBoardPosition;

        if (_moveData.DisplacedPiece != null)
            _moveData.DisplacedPiece.SetActive(true);
    }
}

