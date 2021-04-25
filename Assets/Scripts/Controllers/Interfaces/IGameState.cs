using System;
using System.Collections.Generic;

public interface IGameState
{
    PieceColour Turn { get; }
    IBoardState CurrentBoardState { get; }
    IDictionary<IBoardPosition, HashSet<IBoardPosition>> PossiblePieceMoves { get; }
    void UpdateGameState(IBoardState newState);

}