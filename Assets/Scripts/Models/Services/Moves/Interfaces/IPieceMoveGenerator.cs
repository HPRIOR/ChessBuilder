using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IPieceMoveGenerator
    {
        List<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState);
    }
}