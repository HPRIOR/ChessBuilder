using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildMoves
    {
        // Add representation of colour in build moves
        public BuildMoves(List<Position> buildPositions, List<PieceType> buildPieces)
        {
            BuildPositions = buildPositions;
            BuildPieces = buildPieces;
        }

        public readonly List<PieceType> BuildPieces;
        public readonly List<Position> BuildPositions;
    }
}