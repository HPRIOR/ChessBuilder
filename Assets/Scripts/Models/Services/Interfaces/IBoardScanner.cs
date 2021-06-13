using System.Collections.Generic;
using Models.Services.Moves.MoveHelpers;
using Models.State.Board;

namespace Models.Services.Interfaces
{
    public interface IBoardScanner
    {
        IEnumerable<Position> ScanIn(Direction direction, Position fromPosition, BoardState boardState);
    }
}