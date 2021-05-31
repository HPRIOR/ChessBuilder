using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface ICheckedStateManager
    {
        bool IsCheck { get; }

        void EvaluateCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition);

        IDictionary<BoardPosition, HashSet<BoardPosition>> UpdatePossibleMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition);
    }
}