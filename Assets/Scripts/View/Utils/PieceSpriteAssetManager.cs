using System.Collections.Generic;
using Models.State.PieceState;

namespace View.Utils
{
    internal static class PieceSpriteAssetManager
    {
        private static readonly IDictionary<PieceType, string> PieceSpriteAssetManagerDict =
            new Dictionary<PieceType, string>
            {
                {PieceType.BlackPawn, "Sprites/ChessPieces/240x240/240px-Chess_pdt45.svg"},
                {PieceType.BlackBishop, "Sprites/ChessPieces/240x240/240px-Chess_bdt45.svg"},
                {PieceType.BlackKnight, "Sprites/ChessPieces/240x240/240px-Chess_ndt45.svg"},
                {PieceType.BlackRook, "Sprites/ChessPieces/240x240/240px-Chess_rdt45.svg"},
                {PieceType.BlackKing, "Sprites/ChessPieces/240x240/240px-Chess_kdt45.svg"},
                {PieceType.BlackQueen, "Sprites/ChessPieces/240x240/240px-Chess_qdt45.svg"},
                {PieceType.WhitePawn, "Sprites/ChessPieces/240x240/240px-Chess_plt45.svg"},
                {PieceType.WhiteBishop, "Sprites/ChessPieces/240x240/240px-Chess_blt45.svg"},
                {PieceType.WhiteKnight, "Sprites/ChessPieces/240x240/240px-Chess_nlt45.svg"},
                {PieceType.WhiteRook, "Sprites/ChessPieces/240x240/240px-Chess_rlt45.svg"},
                {PieceType.WhiteKing, "Sprites/ChessPieces/240x240/240px-Chess_klt45.svg"},
                {PieceType.WhiteQueen, "Sprites/ChessPieces/240x240/240px-Chess_qlt45.svg"},
                {PieceType.NullPiece, ""}
            };

        public static string GetSpriteAsset(PieceType pieceType) => PieceSpriteAssetManagerDict[pieceType];
    }
}