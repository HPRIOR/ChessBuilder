using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface ICheckedState
    {
        bool IsTrue { get; }

        IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves, BoardPosition kingPosition);
    }
}