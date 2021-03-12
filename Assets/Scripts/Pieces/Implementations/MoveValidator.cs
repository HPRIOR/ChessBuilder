using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveValidator : IMoveValidator
{
    private static GameController _gameController =
        GameObject
        .FindGameObjectWithTag("GameController")
        .GetComponent<GameController>();


    public bool ValidateMove(GameObject piece, IBoardPosition destination)
    {
        if ((Vector2)piece.transform.position == destination.Position) return false;
        return true;

        // return PieceCanMove(piece, destination);
    }

    private bool PieceCanMove(GameObject piece, IBoardPosition destination) =>
        _gameController.PossibleMoves[piece].Contains(destination);



}
