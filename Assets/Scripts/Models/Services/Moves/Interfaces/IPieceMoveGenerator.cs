using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IPieceMoveGenerator
    {
        IEnumerable<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState);
    }
}