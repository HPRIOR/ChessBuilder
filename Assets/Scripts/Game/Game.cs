using UnityEngine;
using Zenject;

public class Game : MonoBehaviour
{
    private IPieceSpawner _pieceInjector;
    private IPossibleBoardMovesGenerator _possibleBoardMovesGenerator;

    [Inject]
    public void Construct(IPieceSpawner pieceInjector, IPossibleBoardMovesGenerator possibleBoardMovesGenerator)
    {
        _pieceInjector = pieceInjector;
        _possibleBoardMovesGenerator = possibleBoardMovesGenerator;
    }

    public void Start()
    {
        //_pieceInjector.CreatePieceOf(PieceType.BlackKing, new BoardPosition(0, 0));
        //_pieceInjector.CreatePieceOf(PieceType.WhiteKing, new BoardPosition(7, 7));
        _pieceInjector.CreatePieceOf(PieceType.WhitePawn, new BoardPosition(0, 1));
        _pieceInjector.CreatePieceOf(PieceType.BlackPawn, new BoardPosition(1, 2));
        _pieceInjector.CreatePieceOf(PieceType.BlackRook, new BoardPosition(1, 7));
        _pieceInjector.CreatePieceOf(PieceType.BlackKnight, new BoardPosition(5, 5));
        _possibleBoardMovesGenerator.GeneratePossibleMoves();
    }
}