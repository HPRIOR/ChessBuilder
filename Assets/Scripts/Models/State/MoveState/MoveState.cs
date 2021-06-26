using System.Collections.Generic;
using Models.State.Board;

namespace Models.State.MoveState
{
    public readonly struct MoveState
    {
        public IDictionary<Position, HashSet<Position>> PossibleMoves { get; }
        public bool Check { get; }

        public MoveState(IDictionary<Position, HashSet<Position>> possibleMoves, bool check)
        {
            PossibleMoves = possibleMoves;
            Check = check;
        }
    }
}