using Models.State.Piece;

namespace Models.State.Interfaces
{
    public interface IPieceInfo
    {
        string SpriteAssetPath { get; }
        PieceType PieceType { get; }
        PieceColour PieceColour { get; }
    }
}