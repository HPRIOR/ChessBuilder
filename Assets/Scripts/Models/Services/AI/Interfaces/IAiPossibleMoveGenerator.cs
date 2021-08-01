using System;
using System.Collections.Generic;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI.Interfaces
{
    public interface IAiPossibleMoveGenerator
    {
        IEnumerable<Func<GameState, PieceColour, GameState>> GenerateMoves(GameState gameState);
    }
}