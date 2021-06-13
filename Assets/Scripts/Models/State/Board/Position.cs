using UnityEngine;

namespace Models.State.Board
{
    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }
        public Vector2 Vector { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
            Vector = new Vector2(x + 0.5f, y + 0.5f);
        }

        public override string ToString() => $"{X}, {Y}";

        public Position Add(Position position) =>
            new Position(X + position.X, Y + position.Y);

        public bool Equals(Position comparedPosition) =>
            comparedPosition.X == X && comparedPosition.Y == Y;
    }
}