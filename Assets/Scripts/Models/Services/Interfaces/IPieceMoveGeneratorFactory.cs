using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IPieceMoveGeneratorFactory
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType);
    }
}