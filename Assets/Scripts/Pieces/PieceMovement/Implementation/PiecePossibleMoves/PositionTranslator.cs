using System;
using Zenject;

public class PositionTranslator : IPositionTranslator
{
    private readonly PieceColour _pieceColour;
    private readonly IBoardState _boardState;

    public PositionTranslator(PieceColour pieceColour, IBoardState boardState)
    {
        _pieceColour = pieceColour;
        _boardState = boardState;
    }

    public IBoardPosition GetRelativePosition(IBoardPosition originalPosition) =>
        _pieceColour == PieceColour.White
        ? originalPosition
        : new BoardPosition(Math.Abs(originalPosition.X - 7), Math.Abs(originalPosition.Y - 7));

    public ITile GetRelativeTileAt(IBoardPosition boardPosition) =>
        _pieceColour == PieceColour.White
        ? _boardState.GetTileAt(boardPosition)
        : _boardState.GetMirroredTileAt(boardPosition);

    public class Factory : PlaceholderFactory<PieceColour, PositionTranslator> { }
}