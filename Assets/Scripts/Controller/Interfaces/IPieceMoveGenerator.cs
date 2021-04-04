using System.Collections.Generic;

public interface IPieceMoveGenerator
{
    IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState);
}