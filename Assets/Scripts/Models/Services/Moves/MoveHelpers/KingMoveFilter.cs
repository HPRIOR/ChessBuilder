using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.MoveHelpers
{
    public class KingMoveFilter
    {
        public static void RemoveEnemyMovesFromKingMoves(
            IDictionary<Position, HashSet<Position>> turnMoves,
            IDictionary<Position, HashSet<Position>> enemyMoves,
            Position kingPosition)
        {
            if (!kingPosition.Equals(new Position(8, 8))) // null king check
                foreach (var enemyMove in enemyMoves)
                {
                    // TODO mutate turnMoves instead of creating new hashset each time
                    var kingMoves = turnMoves[kingPosition];
                    turnMoves[kingPosition] = new HashSet<Position>(kingMoves.Except(enemyMove.Value));
                }
        }
    }
}