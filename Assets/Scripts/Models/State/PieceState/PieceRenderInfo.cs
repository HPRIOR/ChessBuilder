using Models.State.Interfaces;
using View.Utils;

namespace Models.State.PieceState
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

        public override string ToString() =>
            $"PieceType: {PieceType} \n";
    }
}