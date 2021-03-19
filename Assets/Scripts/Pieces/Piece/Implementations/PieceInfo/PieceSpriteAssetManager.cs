using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
internal static class PieceSpriteAssetManager {
    private static  IDictionary<PieceType, string> _pieceSpriteAssetManager = 
        new Dictionary<PieceType, string>()
        {
            {PieceType.BlackPawn, "Assets/Sprites/ChessPieces/240x240/240px-Chess_pdt45.svg.png"},
            {PieceType.BlackBishop,"Assets/Sprites/ChessPieces/240x240/240px-Chess_bdt45.svg.png" },
            {PieceType.BlackKnight, "Assets/Sprites/ChessPieces/240x240/240px-Chess_ndt45.svg.png"},
            {PieceType.BlackRook, "Assets/Sprites/ChessPieces/240x240/240px-Chess_rdt45.svg.png" },
            {PieceType.BlackKing, "Assets/Sprites/ChessPieces/240x240/240px-Chess_kdt45.svg.png"},
            {PieceType.BlackQueen, "Assets/Sprites/ChessPieces/240x240/240px-Chess_qdt45.svg.png"},
            {PieceType.WhitePawn, "Assets/Sprites/ChessPieces/240x240/240px-Chess_plt45.svg.png"},
            {PieceType.WhiteBishop, "Assets/Sprites/ChessPieces/240x240/240px-Chess_blt45.svg.png"},
            {PieceType.WhiteKnight, "Assets/Sprites/ChessPieces/240x240/240px-Chess_nlt45.svg.png"},
            {PieceType.WhiteRook, "Assets/Sprites/ChessPieces/240x240/240px-Chess_rlt45.svg.png"},
            {PieceType.WhiteKing, "Assets/Sprites/ChessPieces/240x240/240px-Chess_klt45.svg.png"},
            {PieceType.WhiteQueen, "Assets/Sprites/ChessPieces/240x240/240px-Chess_qlt45.svg.png"}

        };
    public static string GetSpriteAsset(PieceType pieceType) => _pieceSpriteAssetManager[pieceType]; 
}
