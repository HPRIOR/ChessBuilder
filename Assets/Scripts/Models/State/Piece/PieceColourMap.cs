using System.Collections.Generic;

namespace Models.State.Piece
{
    internal static class PieceColourMap
    {
        private static readonly HashSet<PieceType> WhitePieces = new HashSet<PieceType>() {
            PieceType.WhiteBishop,
            PieceType.WhiteKnight,
            PieceType.WhitePawn,
            PieceType.WhiteQueen,
            PieceType.WhiteRook,
            PieceType.WhiteKing
        };

        public static PieceColour GetPieceColour(PieceType pieceType) => WhitePieces.Contains(pieceType) ?
            PieceColour.White : PieceColour.Black;
    }
}