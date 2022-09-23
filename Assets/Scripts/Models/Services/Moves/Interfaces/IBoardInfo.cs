using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface IBoardInfo
    {
        Dictionary<Position, List<Position>> TurnMoves { get; }
        Dictionary<Position, List<Position>> EnemyMoves { get; }
        Position KingPosition { get; }

        void EvaluateBoard(BoardState boardState, PieceColour turn);
    }
}