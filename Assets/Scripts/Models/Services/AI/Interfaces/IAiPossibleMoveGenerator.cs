using System;
using System.Collections.Generic;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI.Interfaces
{
    public interface IAiPossibleMoveGenerator
    {
        IEnumerable<Func<BoardState, PieceColour, GameState>> GenerateMoves(GameState gameState);
    }
}