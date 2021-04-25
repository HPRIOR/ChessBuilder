using Models.State.Piece;

namespace Models.Services.Interfaces
{
    public interface IBoardEvalFactory
    {
        IBoardEval Create(PieceColour pieceColour);
    }
}