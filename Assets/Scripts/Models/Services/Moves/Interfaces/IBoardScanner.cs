using System.Collections.Generic;
using Models.Services.Moves.Utils;
using Models.State.Board;

namespace Models.Services.Moves.Interfaces
{
    public interface IBoardScanner
    {
        void ScanIn(Direction direction, Position fromPosition, BoardState boardState, HashSet<Position> possibleMoves);
    }
}