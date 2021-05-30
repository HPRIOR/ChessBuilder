using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface ICheckedState
    {
        bool IsTrue { get; }

        IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleNonKingMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> possibleMoves, BoardPosition kingPosition);

        IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleKingMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> possibleMoves, BoardPosition kingPosition
        );
    }
}