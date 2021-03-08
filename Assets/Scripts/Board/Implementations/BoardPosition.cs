using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPosition: IBoardPosition
{
    public int X { get; }
    public int Y { get; }
    public Vector2 Position { get; }
    public BoardPosition(int x, int y)
    {
        X = x;
        Y = y;
        Position = new Vector2(x + 0.5f, y + 0.5f);
    }

    // TODO
    public string GetAlgabraicNotation() => X.ToString() + Y.ToString();

}
