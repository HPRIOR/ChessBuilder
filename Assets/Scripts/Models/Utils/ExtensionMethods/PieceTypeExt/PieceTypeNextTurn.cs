using Models.State.PieceState;

namespace Models.Utils.ExtensionMethods.PieceTypeExt
{
    public static class PieceTypeNextTurn
    {
        public static PieceColour NextTurn(this PieceColour pieceColour) =>
            pieceColour == PieceColour.Black ? PieceColour.White : PieceColour.Black;
    }
}