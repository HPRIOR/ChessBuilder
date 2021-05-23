using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Game.Interfaces
{
    public interface IGameState
    {
        PieceColour Turn { get; }
        BoardState CurrentBoardState { get; }
        IDictionary<BoardPosition, HashSet<BoardPosition>> PossiblePieceMoves { get; }
        void UpdateGameState(BoardState newState);
    }
}