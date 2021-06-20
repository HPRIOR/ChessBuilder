using System.Collections.Generic;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Build.BuildMoves
{
    internal class HomeBaseBuildMoveGenerator : IBuildMoveGenerator
    {
        public IDictionary<Position, HashSet<PieceType>> GetPossibleBuildMoves(BoardState boardState, PieceColour turn,
            PlayerState playerState) =>
            turn == PieceColour.White ? WhiteBuildMoveGenerator(boardState) : BlackBuildMoveGenerator(boardState);


        private IDictionary<Position, HashSet<PieceType>> BlackBuildMoveGenerator(BoardState boardState) =>
        (
            from Tile tile in boardState.Board
            where tile.Position.Y == 7 || tile.Position.Y == 6
            select tile.Position
        ).ToDictionary(x => x,
            x => new HashSet<PieceType>
            {
                PieceType.BlackBishop, PieceType.BlackKnight, PieceType.BlackPawn, PieceType.BlackQueen,
                PieceType.BlackRook
            });


        private IDictionary<Position, HashSet<PieceType>> WhiteBuildMoveGenerator(BoardState boardState) =>
        (
            from Tile tile in boardState.Board
            where tile.Position.Y == 0 || tile.Position.Y == 1
            select tile.Position
        ).ToDictionary(x => x,
            x => new HashSet<PieceType>
            {
                PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhitePawn, PieceType.WhiteQueen,
                PieceType.WhiteRook
            });
    }
}