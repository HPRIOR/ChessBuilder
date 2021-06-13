using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Game.Interfaces
{
    public interface IGameState
    {
        PieceColour Turn { get; }
        BoardState CurrentBoardState { get; }
        IDictionary<Position, HashSet<Position>> PossiblePieceMoves { get; }
        void UpdateBoardState(BoardState newState);
        void RetainBoardState();
    }
}