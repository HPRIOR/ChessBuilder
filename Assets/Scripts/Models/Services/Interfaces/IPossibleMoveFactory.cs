namespace Models.Services.Interfaces
{
    public interface IPossibleMoveFactory
    {
        IPieceMoveGenerator GetPossibleMoveGenerator(State.PieceState.Piece piece);
    }
}