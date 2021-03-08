using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public Move(ITile fromTile, ITile toTile)
    {
        FromTile = fromTile;
        ToTile = toTile;
        DisplacedPiece = ToTile.CurrentPiece;
        MovedPiece = fromTile.CurrentPiece;
    }
    public ITile FromTile { get; private set; }
    public ITile ToTile { get; private set; }
    public PieceType DisplacedPiece { get; private set; }
    public PieceType MovedPiece { get; private set; }

}
