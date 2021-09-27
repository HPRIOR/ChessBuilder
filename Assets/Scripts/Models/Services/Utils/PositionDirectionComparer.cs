using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Utils
{
    public class PositionDirectionComparer : IEqualityComparer<PositionDirection>
    {
        public bool Equals(PositionDirection x, PositionDirection y)
            => x.Equals(y);

        public int GetHashCode(PositionDirection obj) => obj.GetHashCode();
    }
}