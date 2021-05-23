using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IPieceMoveGenerator
    {
        IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState);
    }
}