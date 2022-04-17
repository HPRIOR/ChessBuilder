using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public sealed class NullMoveGenerator : IPieceMoveGenerator
    {
        public List<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState) =>
            new List<Position>();
    }
}