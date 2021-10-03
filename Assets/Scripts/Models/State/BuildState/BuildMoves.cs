﻿using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildMoves
    {
        public BuildMoves(List<Position> buildPositions, List<PieceType> buildPieces)
        {
            BuildPositions = buildPositions;
            BuildPieces = buildPieces;
        }

        public List<PieceType> BuildPieces { get; }
        public List<Position> BuildPositions { get; }
    }
}