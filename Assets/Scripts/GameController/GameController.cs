using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    private IBoardGenerator _boardLogicGenerator;
    private IPieceGenerator _pieceGenerator;
    public ITile[,] Board { get; private set; }
    public IDictionary<ITile, IBoardPosition> PossibleMoves { get; private set; }
    public PieceColour Turn { get; private set; } = PieceColour.White;

    private void Start()
    {
        Board = _boardLogicGenerator.GenerateBoard();
        _pieceGenerator.GeneratePiece(Board[0, 0], PieceType.BlackKing);
        _pieceGenerator.GeneratePiece(Board[7, 7], PieceType.WhiteKing);
        EvaluateBoardMoves();
    }

    [Inject]
    private void Constructor(IBoardGenerator boardLogicGenerator, IPieceGenerator pieceGenerator)
    {
        _boardLogicGenerator = boardLogicGenerator;
        _pieceGenerator = pieceGenerator;
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