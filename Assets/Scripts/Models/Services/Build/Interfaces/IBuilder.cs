using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Build.Interfaces
{
    public interface IBuilder
    {
        void GenerateNewBoardState(BoardState boardState, Position buildPosition,
            PieceType piece);
    }
}