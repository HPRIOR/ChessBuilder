using System;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.Piece;
using Zenject;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class PositionTranslator : IPositionTranslator
    {
        private readonly PieceColour _pieceColour;

        public PositionTranslator(PieceColour pieceColour)
        {
            _pieceColour = pieceColour;
        }

        public IBoardPosition GetRelativePosition(IBoardPosition originalPosition) =>
            _pieceColour == PieceColour.White
                ? originalPosition
                : new BoardPosition(Math.Abs(originalPosition.X - 7), Math.Abs(originalPosition.Y - 7));

        public ITile GetRelativeTileAt(IBoardPosition boardPosition, IBoardState boardState) =>
            _pieceColour == PieceColour.White
                ? boardState.GetTileAt(boardPosition)
                : boardState.GetMirroredTileAt(boardPosition);

        public class Factory : PlaceholderFactory<PieceColour, PositionTranslator> { }
    }
}