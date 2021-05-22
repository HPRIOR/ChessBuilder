using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IPossibleMoveFactory
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType);
    }
}