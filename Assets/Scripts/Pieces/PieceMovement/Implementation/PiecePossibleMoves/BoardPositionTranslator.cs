using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardPositionTranslator : IBoardPositionTranslator
{
    private readonly PieceColour _pieceColour;
    private readonly IBoardState _boardState;
    public BoardPositionTranslator(PieceColour pieceColour, IBoardState boardState)
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
}
