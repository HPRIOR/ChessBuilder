using System;
using Zenject;

public class PositionTranslator : IPositionTranslator
{
    private readonly PieceColour _pieceColour;

    public PositionTranslator(PieceColour pieceColour, IBoardState boardState)
    {
        _pieceColour = pieceColour;
    }

    public IBoardPosition GetRelativePosition(IBoardPosition originalPosition) =>
        _pieceColour == PieceColour.White
        ? originalPosition
        : new BoardPosition(Math.Abs(originalPosition.X - 7), Math.Abs(originalPosition.Y - 7));

    public ITile GetRelativeTileAt(IBoardPosition boardPosition) => new Tile(new BoardPosition(1, 2));
        //_pieceColour == PieceColour.White
        //? _boardState.GetTileAt(boardPosition)
        //: _boardState.GetMirroredTileAt(boardPosition);

    public class Factory : PlaceholderFactory<PieceColour, PositionTranslator> { }
}