using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Controllers.Builders
{
    public class Builder : IBuilder
    {
        public BoardState
            GenerateNewBoardState(BoardState previousBoardState, Position buildPosition, PieceType piece) =>
            new BoardState();
    }
}