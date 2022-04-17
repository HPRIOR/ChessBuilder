using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface IPositionTranslatorFactory
    {
        IPositionTranslator Create(PieceColour pieceColour);
    }
}