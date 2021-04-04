using UnityEngine;
using Zenject;

public class Game : MonoBehaviour
{
    private IBoardGenerator _boardGenerator;
    public IGameState CurrentState { get; private set; }

    [Inject]
    public void Construct(
        IGameState initState,
        IBoardGenerator boardGenerator
        )
    {
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
        board[3, 3].CurrentPiece = PieceType.WhiteQueen;
        board[4, 4].CurrentPiece = PieceType.BlackQueen;
        return new BoardState(board);
    }
}