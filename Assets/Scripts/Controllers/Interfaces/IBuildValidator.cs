using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Controllers.Interfaces
{
    public interface IBuildValidator
    {
        bool ValidateBuild(BuildMoves buildMoves, Position buildPosition, PieceType buildPiece);
    }
}