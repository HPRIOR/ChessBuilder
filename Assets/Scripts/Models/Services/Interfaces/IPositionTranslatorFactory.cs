using Models.State.Piece;

namespace Models.Services.Interfaces
{
    public interface IPositionTranslatorFactory
    {
        IPositionTranslator Create(PieceColour pieceColour);
    }
}