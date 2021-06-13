using System;
using Models.State.PieceState;

namespace Models.State.Board
{
    public class Tile : ICloneable
    {
        private Tile(Position position, Piece currentPiece)
        {
            Position = position;
            CurrentPiece = currentPiece;
        }

        public Tile(Position position)
        {
            Position = position;
            CurrentPiece = new Piece(PieceType.NullPiece);
        }

        public Piece CurrentPiece { get; set; }
        public Position Position { get; }

        public object Clone() => new Tile(Position, CurrentPiece);

        public override string ToString() =>
            $"Tile at ({Position.X}, {Position.Y}) containing" +
            $" {CurrentPiece}";
    }
}