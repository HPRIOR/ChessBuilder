using UnityEngine;

namespace Models.State.Interfaces
{
    public interface IBoardPosition
    {
        int X { get; }
        int Y { get; }
        Vector2 Vector { get; }

        string GetAlgebraicNotation();

        IBoardPosition Add(IBoardPosition boardPosition);

        bool Equals(IBoardPosition comparedBoardPosition);
    }
}