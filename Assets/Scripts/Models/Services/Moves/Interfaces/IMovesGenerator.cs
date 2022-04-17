using Models.State.Board;
using Models.State.MoveState;
using Models.State.PieceState;

namespace Models.Services.Moves.Interfaces
{
    public interface IMovesGenerator
    {
        MoveState GetPossibleMoves(BoardState boardState, PieceColour turn);
    }
}