using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.Utils
{
    public sealed class KingMoveFilter
    {
        public static void RemoveEnemyMovesFromKingMoves(IDictionary<Position, List<Position>> turnMoves,
            IDictionary<Position, List<Position>> enemyMoves,
            Position kingPosition)
        {
            if (kingPosition != new Position(8, 8)) // null king check
            {
                var kingMoves = new HashSet<Position>(turnMoves[kingPosition]);

                foreach (var keyVal in enemyMoves)
                    for (var index = 0; index < keyVal.Value.Count; index++)
                    {
                        var enemyMove = keyVal.Value[index];
                        if (kingMoves.Contains(enemyMove)) kingMoves.Remove(enemyMove);
                    }

                turnMoves[kingPosition] = kingMoves.ToList();
            }
        }
    }
}