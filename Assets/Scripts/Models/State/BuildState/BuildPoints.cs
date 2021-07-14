using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    // use extension method on PieceType instead
    public static class BuildPoints
    {
        public static readonly IDictionary<PieceType, int> PieceCost = new Dictionary<PieceType, int>
        {
            {PieceType.NullPiece, 0},
            {PieceType.BlackKing, 0},
            {PieceType.WhiteKing, 0},
            {PieceType.BlackPawn, 1},
            {PieceType.WhitePawn, 1},
            {PieceType.BlackBishop, 3},
            {PieceType.WhiteBishop, 3},
            {PieceType.BlackKnight, 3},
            {PieceType.WhiteKnight, 3},
            {PieceType.BlackRook, 5},
            {PieceType.WhiteRook, 5},
            {PieceType.BlackQueen, 9},
            {PieceType.WhiteQueen, 9}
        };
    }
}