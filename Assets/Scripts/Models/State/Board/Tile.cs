using System;
using Models.State.PieceState;

namespace Models.State.Board
{
    public class Tile : ICloneable
    {
        private Tile(BoardPosition boardPosition, Piece currentPiece)
        {
            BoardPosition = boardPosition;
            CurrentPiece = currentPiece;
        }

        public Tile(BoardPosition boardPosition)
        {
            BoardPosition = boardPosition;
            CurrentPiece = new Piece(PieceType.NullPiece);
        }

        public Piece CurrentPiece { get; set; }
        public BoardPosition BoardPosition { get; }

        public object Clone()
        {
            return new Tile(BoardPosition, CurrentPiece);
        }

        public override string ToString()
        {
            return $"Tile at ({BoardPosition.X}, {BoardPosition.Y}) containing" +
                   $" {CurrentPiece}";
        }
    }
}