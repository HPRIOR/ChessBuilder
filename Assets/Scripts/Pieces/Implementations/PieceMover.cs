using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMover : MonoBehaviour
{
    private PieceColour turn = PieceColour.White;
    private GameController _gameController;
    private BoardController _boardController;
    private HashSet<PieceType> _blackPieces = new HashSet<PieceType>()
    {
        PieceType.BlackBishop,
        PieceType.BlackKing,
        PieceType.BlackPawn,
        PieceType.BlackQueen,
        PieceType.BlackRook,
        PieceType.BlackKnight
    };

    private HashSet<PieceType> _whitePieces = new HashSet<PieceType>()
    {
        PieceType.WhiteBishop,
        PieceType.WhiteKing,
        PieceType.WhitePawn,
        PieceType.WhiteQueen,
        PieceType.WhiteRook,
        PieceType.WhiteKnight
    };

    void Start()
    {
        _gameController = GetComponent<GameController>();
        _boardController = GetComponent<BoardController>();
    }

    // references _boardController.PossibleMoves + _gameController.Turn
    private bool CanMove(IPieceInfo pieceInfo, IBoardPosition destination)
    {
        var pieceColoursTurn =
            turn == PieceColour.White & _whitePieces.Contains(pieceInfo.PieceType)
            || turn == PieceColour.Black & _blackPieces.Contains(pieceInfo.PieceType);

        if (!pieceColoursTurn) return false;
        if (_boardController.PossibleMoves[pieceInfo].Length == 0) return false;
        return true;
    }

    // signals to caller that move has taken place (used by drag and drop) while updating board state
    public bool Move(IPieceInfo pieceInfo, IBoardPosition newBoardPosition)
    {
        if (!CanMove(pieceInfo, newBoardPosition)) return false;
        pieceInfo.boardPosition = newBoardPosition;
        _boardController.UpdateBoardState(pieceInfo, newBoardPosition);
        _boardController.EvaluateBoardMoves();
        _gameController.EvaluateGame();
        return true;
    }

    public void ChangeTurn() =>
        _ = turn == PieceColour.White ? turn = PieceColour.Black : PieceColour.White;


}
 
