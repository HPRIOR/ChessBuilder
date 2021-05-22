using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IPositionTranslatorFactory
    {
        IPositionTranslator Create(PieceColour pieceColour);
    }
}