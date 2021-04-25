using System;
using System.Collections.Generic;

public class GameStateController : IGameState, ITurnEventInvoker
{
    private readonly IPossibleMovesGenerator _possibleMovesGenerator;


    public GameStateController(IPossibleMovesGenerator possibleMovesGenerator)
    {
        _possibleMovesGenerator = possibleMovesGenerator;
    }

    public IBoardState CurrentBoardState { get; private set; }

    public IDictionary<IBoardPosition, HashSet<IBoardPosition>> PossiblePieceMoves { get; private set; }

    public PieceColour Turn { get; private set; } = PieceColour.White;

    // provide an overload which passes in the changed tile. This can be used to check for mate when passed
    // to generate possible piece moves
    public void UpdateGameState(IBoardState newState)
    {
        var previousState = CurrentBoardState;
        CurrentBoardState = newState;

        PossiblePieceMoves = _possibleMovesGenerator.GeneratePossibleMoves(CurrentBoardState, Turn);

        Turn = ChangeTurn();
        GameStateChangeEvent?.Invoke(previousState, CurrentBoardState);

    }

    private PieceColour ChangeTurn() =>
        Turn == PieceColour.White ? PieceColour.Black : PieceColour.White; 

    public event Action<IBoardState, IBoardState> GameStateChangeEvent;
}