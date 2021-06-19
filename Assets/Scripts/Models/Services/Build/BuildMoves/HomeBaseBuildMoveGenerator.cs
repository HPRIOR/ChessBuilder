using System;
using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.BuildMoves
{
    internal class HomeBaseBuildMoveGenerator : IBuildMoveGenerator
    {
        public IEnumerable<Position> GetPossibleBuildMoves(BoardState currentBoardState, PieceColour turn) =>
            throw new NotImplementedException();

        private IEnumerable<Position> BlackBuildMoveGenerator(BoardState boardState) =>
            throw new NotImplementedException();
    }
}