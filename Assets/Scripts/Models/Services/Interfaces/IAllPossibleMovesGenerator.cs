using System.Collections.Generic;
using Models.State.Interfaces;
using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IAllPossibleMovesGenerator
    {
        IDictionary<IBoardPosition, HashSet<IBoardPosition>> GetPossibleMoves(IBoardState boardState, PieceColour turn);
    }
}