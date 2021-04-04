using System.Collections.Generic;

public interface IPossibleMovesGenerator
{
    IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState);
}