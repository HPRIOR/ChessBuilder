using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PieceInjector
{
    IBoardState _boardState;
    private readonly Piece.Factory _pieceFactory;

    public PieceInjector(Piece.Factory pieceFactory, IBoardState boardState)
    {
        _boardState = boardState;
        _pieceFactory = pieceFactory;
    }

    public void CreatePieceOf(PieceType pieceType, IBoardPosition BoardPosition)
    {
        var piece = _pieceFactory.Create(pieceType, BoardPosition);
        _boardState.GetTileAt(BoardPosition).CurrentPiece = piece;
    }
}

