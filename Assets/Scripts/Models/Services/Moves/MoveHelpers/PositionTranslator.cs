using System;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveHelpers
{
    public class PositionTranslator : IPositionTranslator
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

        public Tile GetRelativeTileAt(Position position, BoardState boardState) =>
            _pieceColour == PieceColour.White
                ? boardState.Board[position.X, position.Y]
                : boardState.Board[Math.Abs(position.X - 7), Math.Abs(position.Y - 7)];

        public class Factory : PlaceholderFactory<PieceColour, PositionTranslator>
        {
        }
    }
}