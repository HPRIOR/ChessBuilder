using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

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
            ? new State.BuildState.BuildMoves(GetBlackPositions(), RemovePiecesByCost(playerState, BlackPieces))
            : new State.BuildState.BuildMoves(GetWhitePositions(), RemovePiecesByCost(playerState, WhitePieces));


        private static List<PieceType> RemovePiecesByCost(PlayerState playerState,
            IEnumerable<PieceType> pieces)
            => GetAvailablePieces(pieces, playerState); // expensive

        private static List<PieceType> GetAvailablePieces(IEnumerable<PieceType> pieces, PlayerState playerState)
        {
            var result = new List<PieceType>();
            foreach (var piece in pieces)
                if (piece.Value() <= playerState.BuildPoints)
                    result.Add(piece);
            return result;
        }

        private static List<Position> GetWhitePositions()
        {
            var builder = new List<Position>();
            for (var y = 0; y < 2; y++)
            for (var x = 0; x < 8; x++)
                builder.Add(new Position(x, y)); // expensive
            return builder;
        }


        private static List<Position> GetBlackPositions()
        {
            var builder = new List<Position>();
            for (var x = 0; x < 8; x++)
            for (var y = 7; y > 5; y--)
                builder.Add(new Position(x, y));
            return builder;
        }
    }
}