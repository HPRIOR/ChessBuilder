using System.Collections.Generic;
using Models.State.Interfaces;

namespace Models.Services.Interfaces
{
    public interface IPieceMoveGenerator
    {
        IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState);
    }
}