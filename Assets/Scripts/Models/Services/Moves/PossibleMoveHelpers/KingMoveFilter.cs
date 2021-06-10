using System.Collections.Generic;
using System.Linq;
using Models.State.Board;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class KingMoveFilter
    {
        public void RemoveNonTurnMovesFromKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition,
            BoardState boardState)
        {
            foreach (var nonTurnMove in nonTurnMoves)
                RemoveNonPawnNonTurnMovesFromKingMoves(turnMoves, kingPosition, nonTurnMove);
        }

        private static void RemoveNonPawnNonTurnMovesFromKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves, BoardPosition kingPosition,
            KeyValuePair<BoardPosition, HashSet<BoardPosition>> nonTurnMove)
        {
            var kingMoves = turnMoves[kingPosition];
            turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(nonTurnMove.Value));
        }
    }
}