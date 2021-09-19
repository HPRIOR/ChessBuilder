using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface IBoardInfo
    {
        IDictionary<Position, List<Position>> TurnMoves { get;  }
        IDictionary<Position, List<Position>> EnemyMoves { get;  }
        Position KingPosition { get; }

        void EvaluateBoard(BoardState boardState, PieceColour turn);
    }
}