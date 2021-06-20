using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildMoves
    {
        public BuildMoves(HashSet<Position> buildPositions, HashSet<PieceType> buildPieces)
        {
            BuildPositions = buildPositions;
            BuildPieces = buildPieces;
        }

        public HashSet<PieceType> BuildPieces { get; }
        public HashSet<Position> BuildPositions { get; }
    }
}