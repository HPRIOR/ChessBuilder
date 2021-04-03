using UnityEngine;
using Zenject;

public class MoveValidator : IMoveValidator
{

    /*
     * Changes: board state passed as argument, instead of game object IBoardPosition is used to evaluate piece move
     */
    private IBoardState _boardState;
    private IPossibleBoardMovesGenerator _possibleBoardMovesGenerator;

    [Inject]
    public MoveValidator(IBoardState boardState, IPossibleBoardMovesGenerator possibleBoardMovesGenerator)
    {
        _boardState = boardState;
        _possibleBoardMovesGenerator = possibleBoardMovesGenerator;
    }

    public bool ValidateMove(GameObject piece, IBoardPosition destination)
    {
        if ((Vector2)piece.transform.position == destination.Vector) return false;
        return _possibleBoardMovesGenerator.PossibleMoves[piece].Contains(destination);
    }

    public bool ValidateMove(IBoardPosition origin, IBoardPosition destination)
    {
        if (_boardState.GetTileAt(origin).CurrentPiece == null)
        {
            return false;
        }
        return true;
    }
}