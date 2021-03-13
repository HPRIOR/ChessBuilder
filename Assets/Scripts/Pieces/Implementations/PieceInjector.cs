﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PieceInjector
{
    GameController _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    private readonly Piece.Factory _pieceFactory;

    public PieceInjector(Piece.Factory pieceFactory)
    {
        _pieceFactory = pieceFactory;
    }

    public void CreatePieceOf(PieceType pieceType, IBoardPosition BoardPosition)
    {
        var piece = _pieceFactory.Create(pieceType, BoardPosition);
        _gameController.GetTileAt(BoardPosition).CurrentPiece = piece;
    }
}
