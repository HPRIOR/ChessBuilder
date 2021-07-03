using Models.State.PieceState;
using View.Interfaces;

namespace View.Utils
{
    public class PieceRenderInfo : IPieceRenderInfo
    {
        public PieceRenderInfo(PieceType pieceType)
        {
            SpriteAssetPath = PieceSpriteAssetManager.GetSpriteAsset(pieceType);
            PieceType = pieceType;
        }

        public string SpriteAssetPath { get; }
        public PieceType PieceType { get; }
    }
}