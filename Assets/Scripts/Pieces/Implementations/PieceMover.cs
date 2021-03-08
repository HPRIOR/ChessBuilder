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

    void Start()
    {
        _gameController = GetComponent<GameController>();
        _boardController = GetComponent<BoardController>();
    }

    public bool CanMove(IPiece piece, IBoardPosition boardPosition)
    {
        var pieceColoursTurn =
            _gameController.Turn == PieceColour.White & _whitePieces.Contains(piece.Tile.CurrentPiece)
            || _gameController.Turn == PieceColour.Black & _blackPieces.Contains(piece.Tile.CurrentPiece);
        if (!pieceColoursTurn) return false;
        
        //check for valid moves
        if (_boardController.PossibleMoves[piece.Tile].Count == 0) return false;
        var targetTile = _boardController.GetTileAt(boardPosition);
        return _boardController.PossibleMoves[targetTile].Contains(targetTile);
    }

    public void Move(IPiece piece, IBoardPosition newBoardPosition)
    {
        // update piece information on Board
        var movedToTile = _boardController.GetTileAt(newBoardPosition);
        movedToTile.CurrentPiece = piece.Tile.CurrentPiece;

        // update tile information on piece 
        piece.Tile.CurrentPiece = PieceType.None;
        piece.Tile = movedToTile;

        _boardController.EvaluateBoardMoves();

    }

    public Move GetMoveData(IPiece piece, IBoardPosition newBoardPosition)
    {
        var movedToTile = _boardController.GetTileAt(newBoardPosition);
        var movedFromTile = piece.Tile;
        return new Move(movedFromTile, movedToTile);
    }



}
 
