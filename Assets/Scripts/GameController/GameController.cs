using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    private IBoardGenerator _boardLogicGenerator;
    //private IPieceGenerator _pieceGenerator;
    public ITile[,] Board { get; private set; }
    public IDictionary<GameObject, HashSet<IBoardPosition>> PossibleMoves { get; private set; }
    public PieceColour Turn { get; private set; } = PieceColour.White;
    public ICommandInvoker _commandInvoker;
    public PieceInjector pieceInjector;

    private void Start()
    {
        Board = _boardLogicGenerator.GenerateBoard();
        pieceInjector.CreatePieceOf(PieceType.WhiteKing, new BoardPosition(0, 0));
        pieceInjector.CreatePieceOf(PieceType.BlackKing, new BoardPosition(7, 7));
        EvaluateBoardMoves();
    }

    [Inject]
    private void Constructor(IBoardGenerator boardLogicGenerator, ICommandInvoker commandInvoker, PieceInjector pieceInjector)
    {
        _boardLogicGenerator = boardLogicGenerator;
        _commandInvoker = commandInvoker;
        this.pieceInjector = pieceInjector;
    }

    // mate/check/draw
    public void EvaluateGame()
    {
    }

    public void EvaluateBoardMoves()
    {
        // update possible moves dict
    }

    public void ChangeTurn() =>
        _ = Turn == PieceColour.White ? Turn = PieceColour.Black : Turn = PieceColour.White;

    public ITile GetTileAt(IBoardPosition boardPosition) =>
        Board[boardPosition.X, boardPosition.Y];
}