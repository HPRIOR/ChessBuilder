﻿public class PieceSpawner : IPieceSpawner
{
    private readonly PieceMono.Factory _pieceFactory;

    public PieceSpawner(PieceMono.Factory pieceFactory)
    {
        _pieceFactory = pieceFactory;
    }

    public PieceMono CreatePiece(PieceType pieceType, IBoardPosition boardPosition)
    {
        var piece = _pieceFactory.Create(new PieceInfo(pieceType), boardPosition);
        piece.BoardPosition = boardPosition;
        return piece;
    }
}