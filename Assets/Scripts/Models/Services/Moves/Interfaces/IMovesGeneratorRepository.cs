using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface IMovesGeneratorRepository
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(PieceType piece, bool turnMove);
    }
}