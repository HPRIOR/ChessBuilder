using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PieceInfo : IPieceInfo
{
    public PieceInfo(PieceType pieceType)
    {
        SpriteAsset = PieceSpriteAssetManager.GetSpriteAsset(pieceType);
        PieceColour = PieceColourMap.GetPieceColour(pieceType);
        PieceType = pieceType;
    }
    public string SpriteAsset { get; }
    public PieceType PieceType { get; }
    public PieceColour PieceColour { get; }

    public override string ToString() =>
        $"PieceType: {PieceType} \n" +
        $"PieceColour: {PieceColour}";
    
}
