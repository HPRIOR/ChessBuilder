using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Interfaces;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class NullPossibleMoveGenerator : IPieceMoveGenerator
    {
        public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState) =>
            new List<IBoardPosition>();
    }
}