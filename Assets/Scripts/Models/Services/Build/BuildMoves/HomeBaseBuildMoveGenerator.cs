using System.Collections.Generic;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.BuildMoves
{
    internal class HomeBaseBuildMoveGenerator : IBuildMoveGenerator
    {
        public IEnumerable<Position> GetPossibleBuildMoves(BoardState boardState, PieceColour turn) =>
            turn == PieceColour.White ? WhiteBuildMoveGenerator(boardState) : BlackBuildMoveGenerator(boardState);


        private IEnumerable<Position> BlackBuildMoveGenerator(BoardState boardState) =>
        (
            from Tile tile in boardState.Board
            where tile.Position.Y == 7 || tile.Position.Y == 6
            select tile.Position
        ).ToList();


        private IEnumerable<Position> WhiteBuildMoveGenerator(BoardState boardState) =>
        (
            from Tile tile in boardState.Board
            where tile.Position.Y == 0 || tile.Position.Y == 1
            select tile.Position
        ).ToList();
    }
}