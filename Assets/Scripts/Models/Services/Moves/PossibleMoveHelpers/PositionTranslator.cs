using System;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
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

        public BoardPosition GetRelativePosition(BoardPosition originalPosition)
        {
            return _pieceColour == PieceColour.White
                ? originalPosition
                : new BoardPosition(Math.Abs(originalPosition.X - 7), Math.Abs(originalPosition.Y - 7));
        }

        public Tile GetRelativeTileAt(BoardPosition boardPosition, BoardState boardState)
        {
            return _pieceColour == PieceColour.White
                ? boardState.Board[boardPosition.X, boardPosition.Y]
                : boardState.Board[Math.Abs(boardPosition.X - 7), Math.Abs(boardPosition.Y - 7)];
        }

        public class Factory : PlaceholderFactory<PieceColour, PositionTranslator>
        {
        }
    }
}