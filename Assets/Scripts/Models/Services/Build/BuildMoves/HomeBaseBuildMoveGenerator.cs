using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Build.BuildMoves
{
    internal class HomeBaseBuildMoveGenerator : IBuildMoveGenerator
    {
        private static readonly HashSet<PieceType> BlackPieces = new HashSet<PieceType>
        {
            PieceType.BlackBishop, PieceType.BlackKnight, PieceType.BlackPawn, PieceType.BlackQueen,
            PieceType.BlackRook
        };

        private static readonly HashSet<PieceType> WhitePieces = new HashSet<PieceType>
        {
            PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhitePawn, PieceType.WhiteQueen,
            PieceType.WhiteRook
        };

        public State.BuildState.BuildMoves GetPossibleBuildMoves(BoardState boardState, PieceColour turn,
            PlayerState playerState) => turn == PieceColour.Black
            ? new State.BuildState.BuildMoves(GetBlackPositions(), BlackPieces)
            : new State.BuildState.BuildMoves(GetWhitePositions(), WhitePieces);

        private HashSet<Position> GetWhitePositions()
        {
            var result = new HashSet<Position>();
            for (var y = 0; y < 2; y++)
            for (var x = 0; x < 8; x++)
                result.Add(new Position(x, y));
            return result;
        }


        private HashSet<Position> GetBlackPositions()
        {
            var result = new HashSet<Position>();
            for (var x = 0; x < 8; x++)
            for (var y = 7; y > 5; y--)
                result.Add(new Position(x, y));
            return result;
        }
    }
}