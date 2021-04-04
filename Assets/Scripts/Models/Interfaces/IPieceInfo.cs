public interface IPieceInfo
{
    string SpriteAssetPath { get; }
    PieceType PieceType { get; }
    PieceColour PieceColour { get; }
}