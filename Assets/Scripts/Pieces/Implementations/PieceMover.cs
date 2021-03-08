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

    public bool CanMove(IPieceInfo pieceInfo)
    {
        var pieceColoursTurn =
            turn == PieceColour.White & _whitePieces.Contains(pieceInfo.PieceType)
            || turn == PieceColour.Black & _blackPieces.Contains(pieceInfo.PieceType);

        if (!pieceColoursTurn) return false;
        //if (_boardController.PossibleMoves[pieceInfo].Length == 0) return false;
        return true;
    }

    public void Move(IPiece piece, IBoardPosition newBoardPosition)
    {
    }

    public Move GetMoveData(IPiece piece, IBoardPosition newBoardPosition)
    {
        throw new NotImplementedException();
    }

    public void ChangeTurn() =>
        _ = turn == PieceColour.White ? turn = PieceColour.Black : PieceColour.White;


}
 
