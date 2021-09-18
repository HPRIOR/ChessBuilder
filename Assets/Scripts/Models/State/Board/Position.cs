using System;
using UnityEngine;

namespace Models.State.Board
{
    public readonly struct Position : IEquatable<Position>
    {
        public readonly int X;
        public readonly int Y;
        public Vector2 Vector { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
            Vector = new Vector2(x + 0.5f, y + 0.5f);
        }

        public override string ToString() => $"{X}, {Y}";

        public Position Add(Position other) =>
            new Position(X + other.X, Y + other.Y);

        public bool Equals(Position other) =>
            other.X == X && other.Y == Y;

        public static bool operator ==(Position thisPos, Position other) => thisPos.Equals(other);

        public static bool operator !=(Position thisPos, Position other) => !thisPos.Equals(other);

        public override bool Equals(object obj) => obj is Position other && Equals(other);

        public override int GetHashCode() => X * (31 + Y) + Y;
    }
}