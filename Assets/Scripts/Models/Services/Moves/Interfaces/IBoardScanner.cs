using System.Collections.Generic;
using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IBoardScanner
    {
        IEnumerable<Position> ScanIn(Direction direction, Position fromPosition, BoardState boardState);
    }
}