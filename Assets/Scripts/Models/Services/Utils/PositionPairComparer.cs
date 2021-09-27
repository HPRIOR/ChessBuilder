using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Utils
{
    public class PositionPairComparer : IEqualityComparer<PositionPair>
    {
        public bool Equals(PositionPair pp1, PositionPair pp2)
            => pp1.Equals(pp2);

        public int GetHashCode(PositionPair pp) => pp.GetHashCode();
    }
}