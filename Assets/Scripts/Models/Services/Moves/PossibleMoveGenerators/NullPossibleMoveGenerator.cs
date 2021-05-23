using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class NullPossibleMoveGenerator : IPieceMoveGenerator
    {
        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState)
        {
            return new List<BoardPosition>();
        }
    }
}