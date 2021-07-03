using Models.State.PieceState;

namespace View.Interfaces
{
    public interface IPieceRenderInfo
    {
        string SpriteAssetPath { get; }
        PieceType PieceType { get; }
    }
}