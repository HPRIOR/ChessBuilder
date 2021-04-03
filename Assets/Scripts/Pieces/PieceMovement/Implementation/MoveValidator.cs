using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveValidator : IMoveValidator
{

    /*
     * Changes: board state passed as argument, instead of game object IBoardPosition is used to evaluate piece move
     */
    private IPossibleBoardMovesGenerator _possibleBoardMovesGenerator;

    [Inject]
    public MoveValidator( IPossibleBoardMovesGenerator possibleBoardMovesGenerator)
    {
        _possibleBoardMovesGenerator = possibleBoardMovesGenerator;
    }

    public bool ValidateMove(IDictionary<IBoardPosition, HashSet<IBoardPosition>> possibleMoves, IBoardPosition from, IBoardPosition destination)
    {
        if (from == destination) return false;
        return possibleMoves[from].Contains(destination);
    }

}