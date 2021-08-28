using System;
using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.Utils.ExtensionMethods.PieceTypeExt
{
    public static class PieceTypeColour
    {
        private static readonly HashSet<PieceType> BlackPieces =
            new HashSet<PieceType>
            {
                PieceType.BlackBishop,
                PieceType.BlackKing,
                PieceType.BlackPawn,
                PieceType.BlackRook,
                PieceType.BlackKnight,
                PieceType.BlackQueen
            };


        public static PieceColour Colour(this PieceType pieceType)
        {
            if (pieceType == PieceType.NullPiece)
                throw new Exception("Null Piece Has No Colour");

            return BlackPieces.Contains(pieceType)
                ? PieceColour.Black
                : PieceColour.White;
        }
    }
}