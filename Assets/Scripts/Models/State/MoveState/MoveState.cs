using System.Collections.Generic;
using System.Collections.Immutable;
using Models.State.Board;

namespace Models.State.MoveState
{
    public readonly struct MoveState
    {
        // make immutable
        public ImmutableDictionary<Position, ImmutableHashSet<Position>> PossibleMoves { get; }
        public bool Check { get; }

        public MoveState(IDictionary<Position, HashSet<Position>> possibleMoves, bool check)
        {
            PossibleMoves = possibleMoves.ToImmutableDictionary(keyVal => keyVal.Key,
                keyVal => ImmutableHashSet.CreateRange(keyVal.Value));
            Check = check;
        }
    }
}