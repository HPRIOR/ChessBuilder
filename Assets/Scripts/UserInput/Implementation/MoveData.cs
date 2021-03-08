using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData : IMoveData
{
    public MoveData(ITile fromTile, ITile toTile)
    {
        FromTile = (ITile)fromTile.Clone();
        ToTile = (ITile)toTile.Clone();
    }
    public ITile FromTile { get; }
    public ITile ToTile { get; }
   
}
