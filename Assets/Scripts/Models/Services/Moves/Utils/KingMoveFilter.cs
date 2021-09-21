using System;
using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.Utils
{
    public class KingMoveFilter
    {
        public static void RemoveEnemyMovesFromKingMoves(IDictionary<Position, List<Position>> turnMoves,
            IDictionary<Position, List<Position>> enemyMoves,
            Position kingPosition)
        {
            if (kingPosition != new Position(8, 8)) // null king check
                foreach (var enemyMove in enemyMoves)
                    turnMoves[kingPosition] = turnMoves[kingPosition].Except(enemyMove.Value).ToList();
        }
    }
}