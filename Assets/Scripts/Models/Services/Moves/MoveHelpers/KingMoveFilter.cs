using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.MoveHelpers
{
    public class KingMoveFilter
    {
        public static void RemoveNonTurnMovesFromKingMoves(
            IDictionary<Position, HashSet<Position>> turnMoves,
            IDictionary<Position, HashSet<Position>> nonTurnMoves,
            Position kingPosition)
        {
            if (!kingPosition.Equals(new Position(8, 8)))
                foreach (var nonTurnMove in nonTurnMoves)
                {
                    var kingMoves = turnMoves[kingPosition];
                    turnMoves[kingPosition] = new HashSet<Position>(kingMoves.Except(nonTurnMove.Value));
                }
        }
    }
}