using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuilder
    {
        BoardState GenerateNewBoardState(BoardState previousBoardState, Position buildPosition,
            PieceType piece);
    }
}