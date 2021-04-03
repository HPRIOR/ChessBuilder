using UnityEngine;
using Zenject;

public class Game : MonoBehaviour
{
    private IPossibleMovesGenerator _possibleBoardMovesGenerator;
    private IBoardGenerator _boardGenerator;
    public IGameState CurrentState { get; private set; }

    [Inject]
    public void Construct(
        IGameState initState,
        IBoardGenerator boardGenerator 
        //IPossibleBoardMovesGenerator possibleBoardMovesGenerator
        )
    {
        //_possibleBoardMovesGenerator = possibleBoardMovesGenerator;
        _boardGenerator = boardGenerator;
        CurrentState = initState;
       
    }

    public void Start()
    {
        CurrentState.UpdateGameState(InitBoard());
    }

    private IBoardState InitBoard()
    {
        var board = _boardGenerator.GenerateBoard();
        board[0, 0].CurrentPiece = PieceType.WhiteKnight;
        return new BoardState(board);
    }

}