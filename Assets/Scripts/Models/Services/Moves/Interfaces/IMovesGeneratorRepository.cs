using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface IMovesGeneratorRepository
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(Piece piece, bool turnMove);
    }
}