using System.Collections.Generic;
using System.Linq;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class KingMoveFilter
    {
        public void RemoveNonTurnMovesFromKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition,
            BoardState boardState)
        {
            /*
             * Pawns are a special case. Their normal possible move in the forward direction should be disregarded
             * when accounting for possible king moves. Their attacking moves do count however - so are added here.
             */
            foreach (var nonTurnMove in nonTurnMoves)
            {
                var nonTurnPiecePosition = nonTurnMove.Key;
                var nonTurnPieceType = boardState.Board[nonTurnMove.Key.X, nonTurnMove.Key.Y].CurrentPiece.Type;
                var nonTurnPieceIsBlackPawn = nonTurnPieceType.Equals(PieceType.BlackPawn);
                var nonTurnPieceIsWhitePawn = nonTurnPieceType.Equals(PieceType.WhitePawn);
                var nonPawnPiece = !nonTurnPieceIsBlackPawn && !nonTurnPieceIsWhitePawn;

                if (nonTurnPieceIsBlackPawn)
                    RemoveBlackPawnMovesFromKingMoves(turnMoves, kingPosition, nonTurnPiecePosition);
                else if (nonTurnPieceIsWhitePawn)
                    RemoveWhitePawnMovesFromKingMoves(turnMoves, kingPosition, nonTurnPiecePosition);

                if (nonPawnPiece) RemoveNonPawnNonTurnMovesFromKingMoves(turnMoves, kingPosition, nonTurnMove);
            }
        }

        private static void RemoveNonPawnNonTurnMovesFromKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves, BoardPosition kingPosition,
            KeyValuePair<BoardPosition, HashSet<BoardPosition>> nonTurnMove)
        {
            var kingMoves = turnMoves[kingPosition];
            turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(nonTurnMove.Value));
        }

        private static void RemoveWhitePawnMovesFromKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves, BoardPosition kingPosition,
            BoardPosition nonTurnPiecePosition)
        {
            var kingMoves = turnMoves[kingPosition];
            var whitePawnTakingMoves = new HashSet<BoardPosition>
            {
                nonTurnPiecePosition.Add(Move.In(Direction.NE)),
                nonTurnPiecePosition.Add(Move.In(Direction.NW))
            };
            turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(whitePawnTakingMoves));
        }

        private static void RemoveBlackPawnMovesFromKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves, BoardPosition kingPosition,
            BoardPosition nonTurnPiecePosition)
        {
            var kingMoves = turnMoves[kingPosition];
            var blackPawnTakingMoves = new HashSet<BoardPosition>
            {
                nonTurnPiecePosition.Add(Move.In(Direction.SE)),
                nonTurnPiecePosition.Add(Move.In(Direction.SW))
            };
            turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(blackPawnTakingMoves));
        }
    }
}