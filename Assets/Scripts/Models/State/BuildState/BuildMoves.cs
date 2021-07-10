using System.Collections.Immutable;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildMoves
    {
        public BuildMoves(ImmutableHashSet<Position> buildPositions, ImmutableHashSet<PieceType> buildPieces)
        {
            BuildPositions = buildPositions;
            BuildPieces = buildPieces;
        }

        public ImmutableHashSet<PieceType> BuildPieces { get; }
        public ImmutableHashSet<Position> BuildPositions { get; }
    }
}