using System.Collections.Generic;
using Models.State.Interfaces;
using Models.State.PieceState;

namespace Models.Services.Interfaces
{
    public interface IPossibleMovesGenerator
    {
        IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState, PieceColour turn);
    }
}