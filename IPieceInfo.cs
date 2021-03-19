using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPieceInfo : IPieceInfo
{
    public string SpriteAsset { get; }
    public PieceColour PieceColour { get; }
    public PieceType PieceType { get; }
}
