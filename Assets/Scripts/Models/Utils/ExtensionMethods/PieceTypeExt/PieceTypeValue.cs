using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.Utils.ExtensionMethods.PieceTypeExt
{
    public static class PieceTypeValue
    {
        private static readonly IDictionary<PieceType, int> ValueDict = new Dictionary<PieceType, int>
        {
            { PieceType.NullPiece, 0 },
            { PieceType.BlackKing, 0 },
            { PieceType.WhiteKing, 0 },
            { PieceType.BlackPawn, 1 },
            { PieceType.WhitePawn, 1 },
            { PieceType.BlackBishop, 3 },
            { PieceType.WhiteBishop, 3 },
            { PieceType.BlackKnight, 3 },
            { PieceType.WhiteKnight, 3 },
            { PieceType.BlackRook, 5 },
            { PieceType.WhiteRook, 5 },
            { PieceType.BlackQueen, 9 },
            { PieceType.WhiteQueen, 9 }
        };

        public static int Value(this PieceType thisPiece) => ValueDict[thisPiece];
    }
}