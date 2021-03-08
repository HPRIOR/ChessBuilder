using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile : ICloneable
{
    IBoardPosition BoardPosition { get; set; }
    GameObject CurrentPiece { get; set; }

}
