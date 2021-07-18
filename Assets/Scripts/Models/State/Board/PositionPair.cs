using System;

namespace Models.State.Board
{
    public readonly struct PositionPair : IEquatable<PositionPair>
    {
        private readonly Position _position1;
        private readonly Position _position2;

        public PositionPair(Position position1, Position position2)
        {
            _position1 = position1;
            _position2 = position2;
        }

        public bool Equals(PositionPair other) =>
            _position1.Equals(other._position1) && _position2.Equals(other._position2);

        public override bool Equals(object obj) => obj is PositionPair other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (_position1.GetHashCode() * 397) ^ _position2.GetHashCode();
            }
        }
    }
}