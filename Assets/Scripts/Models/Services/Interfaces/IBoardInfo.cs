using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IBoardInfo
    {
        IDictionary<BoardPosition, HashSet<BoardPosition>> TurnMoves { get; }
        IDictionary<BoardPosition, HashSet<BoardPosition>> NonTurnMoves { get; }
        BoardPosition KingPosition { get; }

        void EvaluateBoard(BoardState boardState, PieceColour turn);
    }
}