namespace Models.State.PieceState
{
    public readonly struct Piece
    {
        public readonly PieceColour Colour;
        public readonly PieceType Type;

        public Piece(PieceType pieceType)
        {
            Colour = PieceColourMap.GetPieceColour(pieceType);
            Type = pieceType;
        }
    }
}