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

    public Piece CreatePieceOf(PieceType pieceType, IBoardPosition boardPosition)
    {
        if (_boardState.GetTileAt(boardPosition).CurrentPiece != null)
        {
            throw new PieceSpawnException(
                $"Tried to spawn {pieceType} at ({boardPosition.X},{boardPosition.Y}) " +
                $"which already contained" +
                $" {_boardState.GetTileAt(boardPosition).CurrentPiece.GetComponent<Piece>().Info.PieceType}"
                );
        }
        var piece = _pieceFactory.Create(new PieceInfo(pieceType), boardPosition);
        _boardState.GetTileAt(boardPosition).CurrentPiece = piece.gameObject;
        return piece;
    }
}

