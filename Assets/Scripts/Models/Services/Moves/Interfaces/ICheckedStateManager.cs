using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface ICheckedStateManager
    {
        bool IsCheck { get; }

        void EvaluateCheck(IDictionary<Position, List<Position>> nonTurnMoves,
            Position kingPosition);

        void UpdatePossibleMovesWhenInCheck(IBoardInfo boardInfo, BoardState boardState);
    }
}