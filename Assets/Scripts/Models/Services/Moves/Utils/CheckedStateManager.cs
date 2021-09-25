using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
using Models.Services.Utils;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.Utils
{
    public class CheckedStateManager : ICheckedStateManager
    {
        private static readonly HashSet<PieceType> ScanningPieces = new HashSet<PieceType>
        {
            PieceType.BlackRook,
            PieceType.WhiteRook,
            PieceType.BlackQueen,
            PieceType.WhiteQueen,
            PieceType.BlackBishop,
            PieceType.WhiteBishop
        };

        private IEnumerable<Position> _checkingPieces;


        public bool IsCheck { get; private set; }


        /// <summary>
        ///     Checks for check
        ///     Updates private fields with checking pieces used to to determine multiple or single check, public property
        ///     isCheck
        /// </summary>
        /// <param name="nonTurnMoves"></param>
        /// <param name="kingPosition"></param>
        public void EvaluateCheck(IDictionary<Position, List<Position>> nonTurnMoves,
            Position kingPosition)
        {
            var checkingPieces = GetCheckingPieces(nonTurnMoves, kingPosition).ToList();
            _checkingPieces = checkingPieces;
            IsCheck = checkingPieces.Any();
        }

        /// <summary>
        ///     Updates turn move dictionary with the possible moves available under check
        /// </summary>
        /// <param name="boardInfo"></param>
        /// <returns></returns>
        public void UpdatePossibleMovesWhenInCheck(IBoardInfo boardInfo, BoardState boardState)
        {
            var checkedWithSinglePiece = _checkingPieces.Count() == 1;
            var checkedWithMultiplePieces = _checkingPieces.Count() > 1;

            if (checkedWithSinglePiece)
                UpdateWithInterceptingMoves(boardInfo.TurnMoves, boardInfo.EnemyMoves, boardInfo.KingPosition);

            if (checkedWithMultiplePieces) RemoveAllNonKingMoves(boardInfo.TurnMoves, boardInfo.KingPosition);

            RemoveEnemyMovesFromKingMoves(boardInfo.TurnMoves, boardInfo.EnemyMoves, boardInfo.KingPosition, boardState);
        }

        private static IEnumerable<Position> GetCheckingPieces(IDictionary<Position, List<Position>> enemyMoves,
            Position kingPosition)
        {
            var result = new List<Position>();
            foreach (var keyValuePair in enemyMoves)
                if (keyValuePair.Value.Contains(kingPosition))
                    result.Add(keyValuePair.Key);
            return result;
        }

        /// <summary>
        ///     Removes positions not between king and checking piece from all non king, turn moves
        /// </summary>
        /// <param name="turnMoves"></param>
        /// <param name="enemyMoves"></param>
        /// <param name="kingPosition"></param>
        private void UpdateWithInterceptingMoves(IDictionary<Position, List<Position>> turnMoves,
            IDictionary<Position, List<Position>> enemyMoves,
            Position kingPosition)
        {
            var positionsBetweenKingAndCheckPiece = GetPositionsBetweenCheckedKing(kingPosition, enemyMoves);
            var turnMovePositions = turnMoves.Keys.ToList();
            foreach (var position in turnMovePositions)
            {
                var notKingPiece = position != kingPosition;
                if (notKingPiece)
                {
                    // keyVal.Value.IntersectWith(positionsBetweenKingAndCheckPiece);
                    turnMoves[position] = turnMoves[position].Intersect(positionsBetweenKingAndCheckPiece).ToList();
                }
            }
        }

        private HashSet<Position> GetPositionsBetweenCheckedKing(Position kingPosition,
            IDictionary<Position, List<Position>> enemyMoves)
        {
            var checkingPiecePosition = _checkingPieces.First();
            var possibleMoves = enemyMoves[checkingPiecePosition];
            foreach (var boardPosition in possibleMoves)
                if (kingPosition == boardPosition)
                {
                    var positionsBetweenCheckedKing = ScanCache.ScanBetween(boardPosition, checkingPiecePosition);
                    var result = new HashSet<Position>(positionsBetweenCheckedKing) { checkingPiecePosition };
                    return result;
                }

            return new HashSet<Position>();
        }

        private void RemoveEnemyMovesFromKingMoves(IDictionary<Position, List<Position>> turnMoves,
            IDictionary<Position, List<Position>> enemyMoves,
            Position kingPosition, BoardState boardState)
        {
            // this won't remove the moves of scanning pieces past king 
            KingMoveFilter.RemoveEnemyMovesFromKingMoves(turnMoves, enemyMoves, kingPosition);

            // extra logic needed here to do that
            foreach (var checkingPiecePosition in _checkingPieces)
                if (ScanningPieces.Contains(PieceAt(checkingPiecePosition, boardState).Type))
                {
                    // remove extended possible moves that go 'through' king
                    var movesExtendedThroughKing =
                        ScanCache.Scan(checkingPiecePosition,
                            DirectionCache.DirectionFrom(checkingPiecePosition, kingPosition));
                    turnMoves[kingPosition] = turnMoves[kingPosition].Except(movesExtendedThroughKing).ToList();
                }
        }


        private static void RemoveAllNonKingMoves(IDictionary<Position, List<Position>> turnMoves,
            Position kingPosition)
        {
            foreach (var turnMove in turnMoves)
                if (turnMove.Key != kingPosition)
                    turnMove.Value.Clear();
        }

        private Piece PieceAt(Position position, BoardState boardState) =>
            boardState.Board[position.X][position.Y].CurrentPiece;
    }
}