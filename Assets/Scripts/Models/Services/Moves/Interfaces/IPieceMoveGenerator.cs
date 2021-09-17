using System.Collections.Generic;
using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IPieceMoveGenerator
    {
        HashSet<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState);
    }
}