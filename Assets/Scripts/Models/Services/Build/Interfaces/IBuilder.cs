using Models.State.Board;

namespace Models.Services.Build.Interfaces
{
    public interface IBuilder
    {
        BoardState GenerateNewBoardState(BoardState previousBoardState, Position buildPosition,
            State.PieceState.Piece piece);
    }
}