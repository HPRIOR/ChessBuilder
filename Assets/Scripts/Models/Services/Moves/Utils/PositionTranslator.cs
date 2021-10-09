using System;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.Utils
{
    public sealed class PositionTranslator : IPositionTranslator
    {
        private readonly PieceColour _pieceColour;

        public PositionTranslator(PieceColour pieceColour)
        {
            _pieceColour = pieceColour;
        }

        public Position GetRelativePosition(Position originalPosition) =>
            _pieceColour == PieceColour.White
                ? originalPosition
                : new Position(Math.Abs(originalPosition.X - 7), Math.Abs(originalPosition.Y - 7));

        public ref Tile GetRelativeTileAt(Position position, BoardState boardState)
        {
            if (_pieceColour == PieceColour.White)
            {
                return ref boardState.GetTileAt(position);
            }

            return ref boardState.GetTileAt(Math.Abs(position.X - 7), Math.Abs(position.Y - 7));
        }

        public sealed class Factory : PlaceholderFactory<PieceColour, PositionTranslator>
        {
        }
    }
}