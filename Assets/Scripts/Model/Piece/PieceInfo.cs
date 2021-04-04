public class PieceInfo : IPieceInfo
{
    public PieceInfo(PieceType pieceType)
    {
        SpriteAssetPath = PieceSpriteAssetManager.GetSpriteAsset(pieceType);
        PieceColour = PieceColourMap.GetPieceColour(pieceType);
        PieceType = pieceType;
    }

    public string SpriteAssetPath { get; }
    public PieceType PieceType { get; }
    public PieceColour PieceColour { get; }

    public override string ToString() =>
        $"PieceType: {PieceType} \n" +
        $"PieceColour: {PieceColour}";
}