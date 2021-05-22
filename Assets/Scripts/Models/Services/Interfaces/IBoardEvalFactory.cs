using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IBoardEvalFactory
    {
        IBoardEval Create(PieceColour pieceColour);
    }
}