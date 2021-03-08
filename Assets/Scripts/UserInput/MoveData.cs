using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData : IMoveData
{
    public MoveData(ITile fromTile, ITile toTile)
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
