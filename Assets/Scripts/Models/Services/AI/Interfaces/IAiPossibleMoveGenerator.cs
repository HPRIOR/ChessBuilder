using System;
using System.Collections.Generic;
using Models.Services.Game.Interfaces;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI.Interfaces
{
    public interface IAiPossibleMoveGenerator
    {
        IEnumerable<Action<PieceColour, IGameStateUpdater>> GenerateMoves(GameState gameState);
    }
}