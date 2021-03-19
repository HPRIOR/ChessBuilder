using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class PieceColourMap
{
    private static HashSet<PieceType> WhitePieces = new HashSet<PieceType>() { 
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
