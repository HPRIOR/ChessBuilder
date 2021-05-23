using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface ITileEvaluatorFactory
    {
        ITileEvaluator Create(PieceColour pieceColour);
    }
}