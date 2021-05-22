using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IBoardEvalFactory
    {
        IBoardMoveEval Create(PieceColour pieceColour);
    }
}