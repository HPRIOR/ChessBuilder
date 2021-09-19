using System.Collections.Generic;

namespace Models.State.PieceState
{
    public class PieceTypeComparer : IEqualityComparer<PieceType>
    {
        public bool Equals(PieceType x, PieceType y) => x == y;

        public int GetHashCode(PieceType obj) => (int)obj;
    }
}