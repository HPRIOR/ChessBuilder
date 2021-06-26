using System;
using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.Utils.ExtensionMethods.PieceType
{
    public static class PieceTypeColour
    {
        private static readonly HashSet<State.PieceState.PieceType> BlackPieces =
            new HashSet<State.PieceState.PieceType>
            {
                State.PieceState.PieceType.BlackBishop,
                State.PieceState.PieceType.BlackKing,
                State.PieceState.PieceType.BlackPawn,
                State.PieceState.PieceType.BlackRook,
                State.PieceState.PieceType.BlackKnight,
                State.PieceState.PieceType.BlackQueen
            };


        public static PieceColour Colour(this State.PieceState.PieceType pieceType)
        {
            if (pieceType == State.PieceState.PieceType.NullPiece)
                throw new Exception("Null Piece Has No Colour");

            return BlackPieces.Contains(pieceType)
                ? PieceColour.Black
                : PieceColour.White;
        }
    }
}