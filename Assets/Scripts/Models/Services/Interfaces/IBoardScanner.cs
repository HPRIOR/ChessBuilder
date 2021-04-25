using System.Collections.Generic;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IBoardScanner
    {
        IEnumerable<IBoardPosition> ScanIn(Direction direction, IBoardPosition fromPosition, IBoardState boardState);
    }
}