using System.Collections.Generic;

public class NullPossibleMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState) =>
        new List<IBoardPosition>();
}