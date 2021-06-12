using UnityEngine;

namespace Models.State.Board
{
    public readonly struct BoardPosition
    {
        public int X { get; }
        public int Y { get; }
        public Vector2 Vector { get; }

        public BoardPosition(int x, int y)
        {
            X = x;
            Y = y;
            Vector = new Vector2(x + 0.5f, y + 0.5f);
        }

        public override string ToString() => $"{X}, {Y}";

        public BoardPosition Add(BoardPosition boardPosition) =>
            new BoardPosition(X + boardPosition.X, Y + boardPosition.Y);

        public bool Equals(BoardPosition comparedBoardPosition) =>
            comparedBoardPosition.X == X && comparedBoardPosition.Y == Y;
    }
}