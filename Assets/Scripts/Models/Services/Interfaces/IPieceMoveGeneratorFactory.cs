using Models.State.Piece;

namespace Models.Services.Interfaces
{
    public interface IPieceMoveGeneratorFactory
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(PieceType pieceType);
    }
}