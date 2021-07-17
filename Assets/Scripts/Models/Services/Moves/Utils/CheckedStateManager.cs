using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.BoardPos;

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

        private readonly BoardState _boardState;
        private IEnumerable<Position> _checkingPieces;

        public CheckedStateManager(BoardState boardState)
        {
            _boardState = boardState;
        }

        public bool IsCheck { get; private set; }


        /// <summary>
        ///     Checks for check
        ///     Updates private fields with checking pieces used to to determine multiple or single check, public property
        ///     isCheck
        /// </summary>
        /// <param name="nonTurnMoves"></param>
        /// <param name="kingPosition"></param>
        public void EvaluateCheck(IDictionary<Position, HashSet<Position>> nonTurnMoves,
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
        public void UpdatePossibleMovesWhenInCheck(IBoardInfo boardInfo)
        {
            var checkedWithSinglePiece = _checkingPieces.Count() == 1;
            var checkedWithMultiplePieces = _checkingPieces.Count() > 1;

            if (checkedWithSinglePiece)
                UpdateWithInterceptingMoves(boardInfo.TurnMoves, boardInfo.EnemyMoves, boardInfo.KingPosition);

            if (checkedWithMultiplePieces) RemoveAllNonKingMoves(boardInfo.TurnMoves, boardInfo.KingPosition);

            RemoveEnemyMovesFromKingMoves(boardInfo.TurnMoves, boardInfo.EnemyMoves, boardInfo.KingPosition);
        }

        private static IEnumerable<Position> GetCheckingPieces(
            IDictionary<Position, HashSet<Position>> enemyMoves, Position kingPosition)
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
        private void UpdateWithInterceptingMoves(
            IDictionary<Position, HashSet<Position>> turnMoves,
            IDictionary<Position, HashSet<Position>> enemyMoves,
            Position kingPosition)
        {
            var positionsBetweenKingAndCheckPiece = GetPositionsBetweenCheckedKing(kingPosition, enemyMoves);
            foreach (var keyVal in turnMoves)
            {
                var notKingPiece = !keyVal.Key.Equals(kingPosition);
                if (notKingPiece) keyVal.Value.IntersectWith(positionsBetweenKingAndCheckPiece);
            }
        }

        private HashSet<Position> GetPositionsBetweenCheckedKing(Position kingPosition,
            IDictionary<Position, HashSet<Position>> enemyMoves)
        {
            var checkingPiecePosition = _checkingPieces.First();
            var possibleMoves = enemyMoves[checkingPiecePosition];
            foreach (var boardPosition in possibleMoves)
                if (kingPosition.Equals(boardPosition))
                {
                    var result = boardPosition.ScanBetween(checkingPiecePosition);
                    (result as List<Position>)?.Add(checkingPiecePosition);
                    return new HashSet<Position>(result);
                }

            return new HashSet<Position>();
        }

        private void RemoveEnemyMovesFromKingMoves(
            IDictionary<Position, HashSet<Position>> turnMoves,
            IDictionary<Position, HashSet<Position>> enemyMoves,
            Position kingPosition)
        {
            // this won't remove the moves of scanning pieces past king 
            KingMoveFilter.RemoveEnemyMovesFromKingMoves(turnMoves, enemyMoves, kingPosition);

            // extra logic needed here to do that
            foreach (var checkingPiecePosition in _checkingPieces)
                if (ScanningPieces.Contains(PieceAt(checkingPiecePosition).Type))
                {
                    // remove extended possible moves that go 'through' king
                    var movesExtendedThroughKing =
                        checkingPiecePosition.Scan(checkingPiecePosition.DirectionTo(kingPosition));
                    turnMoves[kingPosition].ExceptWith(movesExtendedThroughKing);
                }
        }


        private static void RemoveAllNonKingMoves(
            IDictionary<Position, HashSet<Position>> turnMoves,
            Position kingPosition)
        {
            foreach (var turnMove in turnMoves)
                if (!turnMove.Key.Equals(kingPosition))
                    turnMove.Value.Clear();
        }

        private Piece PieceAt(Position position) =>
            _boardState.Board[position.X, position.Y].CurrentPiece;
    }
}