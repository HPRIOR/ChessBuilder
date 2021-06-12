using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;

namespace Models.Services.Moves.PossibleMoveGenerators.TurnMoves
{
    public class NullMoveGenerator : IPieceMoveGenerator
    {
        public IEnumerable<BoardPosition> GetPossiblePieceMoves(BoardPosition originPosition, BoardState boardState) =>
            new List<BoardPosition>();
    }
}