using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PieceSpawner : IPieceSpawner
{
    IBoardState _boardState;
    private readonly Piece.Factory _pieceFactory;

    public PieceSpawner(Piece.Factory pieceFactory, IBoardState boardState)
    {
        _boardState = boardState;
        _pieceFactory = pieceFactory;
    }

    public void CreatePieceOf(PieceType pieceType, IBoardPosition boardPosition)
    {
        var piece = _pieceFactory.Create(pieceType, new PieceInfo(pieceType), boardPosition);
        _boardState.GetTileAt(boardPosition).CurrentPiece = piece.gameObject;
    }
}

