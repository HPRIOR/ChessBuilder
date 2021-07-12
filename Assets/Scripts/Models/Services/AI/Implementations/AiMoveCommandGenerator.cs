using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Models.Services.AI.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI
{
    public class AiMoveCommandGenerator: IAiCommandGenerator
    {
        public IEnumerable<Func<BoardState, PieceColour, GameState>> GenerateCommands(GameState gameState)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Func<BoardState, PieceColour, GameState>> GetMoveCommands(ImmutableDictionary<Position, ImmutableHashSet<Position>> moves)
        {
            throw new NotImplementedException();
        }
        
        
        private IEnumerable<Func<BoardState, PieceColour, GameState>> GetBuildCommands(BuildMoves builds)
        {
            throw new NotImplementedException();
        }

    }
    
    
}
