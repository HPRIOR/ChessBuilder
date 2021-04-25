using Models.State.Interfaces;
using UnityEngine;

namespace Models.State.Board
{
    public readonly struct BoardPosition : IBoardPosition
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

        // TODO
        public BoardPosition(string algebraicNotation) => throw new System.NotImplementedException();

        // TODO
        public string GetAlgebraicNotation() => X.ToString() + Y.ToString();

        public override string ToString()
        {
            return $"{X}, {Y}";
        }

        public IBoardPosition Add(IBoardPosition boardPosition) => new BoardPosition(X + boardPosition.X, Y + boardPosition.Y);

        public bool Equals(IBoardPosition comparedBoardPosition)
            => comparedBoardPosition.X == X && comparedBoardPosition.Y == Y;
    }
}