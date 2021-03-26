﻿using Zenject;

public class BoardEval : IBoardEval
{
    private readonly PieceColour _pieceColour;

    public BoardEval(PieceColour pieceColour)
    {
        _pieceColour = pieceColour;
    }

    public bool FriendlyPieceIn(ITile tile) =>
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour == _pieceColour;

    public bool OpposingPieceIn(ITile tile) =>
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour != _pieceColour;

    public class Factory : PlaceholderFactory<PieceColour, BoardEval> { }
}