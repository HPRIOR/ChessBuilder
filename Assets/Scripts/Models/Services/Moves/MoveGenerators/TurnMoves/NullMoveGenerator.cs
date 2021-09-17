using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public class NullMoveGenerator : IPieceMoveGenerator
    {
        public HashSet<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState) =>
            new HashSet<Position>();
    }
}