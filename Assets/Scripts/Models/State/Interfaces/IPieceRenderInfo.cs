using Models.State.PieceState;

namespace Models.State.Interfaces
{
    public interface IPieceRenderInfo
    {
        string SpriteAssetPath { get; }
        PieceType PieceType { get; }
    }
}