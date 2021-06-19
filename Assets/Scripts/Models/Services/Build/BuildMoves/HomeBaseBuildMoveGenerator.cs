using System;
using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;

namespace Models.Services.Build.BuildMoves
{
    internal class HomeBaseBuildMoveGenerator : IBuildMoveGenerator
    {
        public IEnumerable<Position> GetPossibleBuildMoves(BoardState currentBoardState, Turn turn) =>
            throw new NotImplementedException();

        private IEnumerable<Position> BlackBuildMoveGenerator() => throw new NotImplementedException();
    }
}