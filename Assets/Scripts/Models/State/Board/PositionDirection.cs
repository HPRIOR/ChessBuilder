using System;
using Models.Services.Moves.Utils;

namespace Models.State.Board
{
    public readonly struct PositionDirection : IEquatable<PositionDirection>
    {
        private readonly Position _position;
        private readonly Direction _direction;

        public PositionDirection(Position position, Direction direction)
        {
            _direction = direction;
            _position = position;
        }

        public bool Equals(PositionDirection other) => _position == other._position && _direction == other._direction;

        public override bool Equals(object obj) => obj is PositionDirection other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (_position.GetHashCode() * 397) ^ (int)_direction;
            }
        }
    }
}