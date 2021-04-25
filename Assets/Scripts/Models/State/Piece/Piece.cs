namespace Models.State.Piece
{
    public readonly struct Piece
    {
        public PieceColour Colour { get; }
        public PieceType Type { get; }
        public Piece(PieceType pieceType)
        {
            Colour = PieceColourMap.GetPieceColour(pieceType);
            Type = pieceType;
        }
    }
}
