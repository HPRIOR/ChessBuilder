using System.Collections.Generic;
using Models.State.Board;

namespace Models.State.MoveState
{
    /// <summary>
    /// Represents the possible moves (not builds) a player can make and if Check has occured
    /// </summary>
    public readonly struct MoveState
    {
        public IDictionary<Position, List<Position>> PossibleMoves { get; }
        public bool Check { get; }

        public MoveState(IDictionary<Position, List<Position>> possibleMoves, bool check)
        {
            PossibleMoves = possibleMoves;
            Check = check;
        }
    }
}