namespace Models.Services.Interfaces
{
    public interface IMovesGeneratorRepository
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(State.PieceState.Piece piece, bool turnMove);
    }
}