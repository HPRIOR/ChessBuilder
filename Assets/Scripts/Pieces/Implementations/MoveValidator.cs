using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MoveValidator : IMoveValidator
{
    private IBoardState _boardState;

    [Inject]
    public MoveValidator(IBoardState boardState)
    {
        _boardState = boardState;
    }
    
    public bool ValidateMove(GameObject piece, IBoardPosition destination)
    {
        if ((Vector2)piece.transform.position == destination.Position) return false;
        return true;

        // return PieceCanMove(piece, destination);
    }

    public bool ValidateMove(IBoardPosition origin, IBoardPosition destination)
    {
        if (_boardState.GetTileAt(origin).CurrentPiece == null)
        {
            return false;
        }
        return true;
    }

    private bool PieceCanMove(GameObject piece, IBoardPosition destination) => true;
        //_gameController.PossibleMoves[piece].Contains(destination);



}
