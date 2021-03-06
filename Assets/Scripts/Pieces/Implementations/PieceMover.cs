using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMover : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GetComponent<GameController>();
        _boardController = GetComponent<BoardController>();

    }

    // references _boardController.PossibleMoves + _gameController.Turn
    private bool CanMove(PieceType pieceType, IBoardPosition destination)
    {
        var pieceColoursTurn =
            _gameController.Turn == PieceColour.White & _whitePieces.Contains(pieceType)
            || _gameController.Turn == PieceColour.Black & _blackPieces.Contains(pieceType);

        throw new NotImplementedException();
    }


    // signals to caller that move has taken place (used by drag and drop) while updating board state
    public bool Move(PieceType piece, IBoardPosition previousBoardPosition, IBoardPosition newBoardPosition)
    {
        throw new NotImplementedException();
    }

}
 
