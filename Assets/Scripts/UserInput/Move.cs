﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public ITile FromTile { get; private set; }
    public ITile ToTile { get; private set; }
    public IPiece DisplacedPiece { get; private set; }
    public IPiece MovedPiece { get; private set; }

}