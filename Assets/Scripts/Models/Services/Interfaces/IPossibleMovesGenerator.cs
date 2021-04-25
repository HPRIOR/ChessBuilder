using System.Collections.Generic;
using Models.State.Interfaces;
using Models.State.Piece;

namespace Models.Services.Interfaces
{
    public interface IPossibleMovesGenerator
    {
        IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState, PieceColour turn);
    }
}