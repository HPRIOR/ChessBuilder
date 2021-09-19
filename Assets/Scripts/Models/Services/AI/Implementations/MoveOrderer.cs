using System.Collections.Generic;
using System.Linq;
using Models.Services.AI.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.AI.Implementations
{
    public class MoveOrderer : IMoveOrderer
    {
        public IEnumerable<AiMove> OrderMoves(IEnumerable<AiMove> moves, BoardState boardState)
        {
            var movePoints = new Dictionary<AiMove, int>();
            foreach (var action in moves) // need some way of storing the location of a move
            {
                movePoints.Add(action,
                    action.MoveType == MoveType.Build
                        ? BuildPoints(action, boardState)
                        : TakePoints(action, boardState));
            }

            // might not be able to find the right value because of immutable struct
            return moves.OrderByDescending(move => movePoints[move]);
        }

        private int TakePoints(AiMove move, BoardState boardState)
        {
            var takingPiece = boardState.Board[move.From.X][move.From.Y].CurrentPiece.Type;
            var takenPiece = boardState.Board[move.To.X][move.To.Y].CurrentPiece.Type;
            if (takenPiece == PieceType.NullPiece)
                return 0;
            return takenPiece.Value() - takingPiece.Value();
        }

        private int BuildPoints(AiMove move, BoardState boardState)
        {
            var buildIsOverwriting =
                boardState.Board[move.From.X][move.From.Y].CurrentPiece.Type != PieceType.NullPiece;
            return buildIsOverwriting ? 0 : move.Type.Value();
        }
    }
}