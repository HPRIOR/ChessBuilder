using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface ITileEvaluatorFactory
    {
        ITileEvaluator Create(PieceColour pieceColour);
    }
}