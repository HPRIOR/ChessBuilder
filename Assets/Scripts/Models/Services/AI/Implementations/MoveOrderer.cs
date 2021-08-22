using System;
using System.Collections.Generic;
using Models.Services.AI.Interfaces;
using Models.Services.Game.Implementations;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.AI.Implementations
{
    public class MoveOrderer : IMoveOrderer
    {
        private readonly GameStateUpdater _gameStateUpdater;

        public MoveOrderer(GameStateUpdater gameStateUpdater)
        {
            _gameStateUpdater = gameStateUpdater;
        }

        public void OrderMoves(IEnumerable<Action<PieceColour>> moves)
        {
            foreach (var action in moves) // need some way of storing the location of a move
            {
            }
        }

        private int GetPointsForMove() => throw new NotImplementedException();

        public class Factory : PlaceholderFactory<GameStateUpdater, MoveOrderer>
        {
        }
    }
}