﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
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
            ? new State.BuildState.BuildMoves(GetBlackPositions(), RemovePiecesByCost(playerState, BlackPieces))
            : new State.BuildState.BuildMoves(GetWhitePositions(), RemovePiecesByCost(playerState, WhitePieces));


        private static ImmutableHashSet<PieceType> RemovePiecesByCost(PlayerState playerState,
            IEnumerable<PieceType> pieces)
        {
            var result = ImmutableHashSet<PieceType>.Empty.ToBuilder();
            var availablePieces = pieces.Where(piece => BuildPoints.PieceCost[piece] <= playerState.BuildPoints);
            availablePieces.ToList().ForEach(piece => result.Add(piece));
            return result.ToImmutable();
        }


        private static ImmutableHashSet<Position> GetWhitePositions()
        {
            var builder = ImmutableHashSet<Position>.Empty.ToBuilder();
            for (var y = 0; y < 2; y++)
            for (var x = 0; x < 8; x++)
                builder.Add(new Position(x, y));
            return builder.ToImmutable();
        }


        private static ImmutableHashSet<Position> GetBlackPositions()
        {
            var builder = ImmutableHashSet<Position>.Empty.ToBuilder();
            for (var x = 0; x < 8; x++)
            for (var y = 7; y > 5; y--)
                builder.Add(new Position(x, y));
            return builder.ToImmutable();
        }
    }
}