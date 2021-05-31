using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IAllPossibleMovesGenerator
    {
        IDictionary<BoardPosition, HashSet<BoardPosition>> GetPossibleMoves(BoardState boardState, PieceColour turn);
    }
}