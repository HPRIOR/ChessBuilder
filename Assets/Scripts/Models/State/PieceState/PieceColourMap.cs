﻿using System.Collections.Generic;

namespace Models.State.PieceState
{
    internal static class PieceColourMap
    {
        private static readonly HashSet<PieceType> WhitePieces = new HashSet<PieceType>(new PieceTypeComparer())
        {
            PieceType.WhiteBishop,
            PieceType.WhiteKnight,
            PieceType.WhitePawn,
            PieceType.WhiteQueen,
            PieceType.WhiteRook,
            PieceType.WhiteKing
        };

        public static PieceColour GetPieceColour(PieceType pieceType) =>
            WhitePieces.Contains(pieceType) ? PieceColour.White : PieceColour.Black;
    }
}