using System.Collections.Generic;

namespace Models.Services.Moves.Utils
{
    public sealed class DirectionComparer : IEqualityComparer<Direction>
    {
        public bool Equals(Direction x, Direction y) => x == y;

        public int GetHashCode(Direction obj) => (int)obj;
    }
}