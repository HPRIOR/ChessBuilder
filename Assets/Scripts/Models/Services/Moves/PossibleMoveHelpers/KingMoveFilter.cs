using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class KingMoveFilter
    {
        public static void RemoveNonTurnMovesFromKingMoves(IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            foreach (var nonTurnMove in nonTurnMoves)
            {
                var kingMoves = turnMoves[kingPosition];
                turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(nonTurnMove.Value));
            }
        }
    }
}