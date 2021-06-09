namespace Models.Services.Interfaces
{
    public interface IMoveGeneratorRepository
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(State.PieceState.Piece piece, bool turnMove);
    }
}