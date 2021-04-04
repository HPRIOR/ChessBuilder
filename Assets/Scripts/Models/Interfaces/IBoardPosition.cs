using UnityEngine;

public interface IBoardPosition
{
    int X { get; }
    int Y { get; }
    Vector2 Vector { get; }

    string GetAlgabraicNotation();

    IBoardPosition Add(IBoardPosition boardPosition);
}