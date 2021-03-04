﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardPosition
{
    int X { get; }
    int Y { get; }
    Vector2 Position { get; }
    string GetAlgabraicNotation();
}
