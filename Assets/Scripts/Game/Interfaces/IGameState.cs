using System.Collections.Generic;
using Models.State.Interfaces;
using Models.State.PieceState;

namespace Game.Interfaces
{
    public interface IGameState
    {
        PieceColour Turn { get; }
        IBoardState CurrentBoardState { get; }
        IDictionary<IBoardPosition, HashSet<IBoardPosition>> PossiblePieceMoves { get; }
        void UpdateGameState(IBoardState newState);

    }
}