using Controllers.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Controllers.Builders
{
    public class BuildValidator : IBuildValidator
    {
        public bool ValidateBuild(BuildMoves buildMoves, Position buildPosition, PieceType buildPiece) =>
            buildMoves.BuildPositions.Contains(buildPosition) && buildMoves.BuildPieces.Contains(buildPiece);
    }
}