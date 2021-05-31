using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.BoardPos;

namespace Models.Services.Moves.PossibleMoveHelpers
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
        private readonly KingMoveFilter _kingMoveFilter;
        private IEnumerable<BoardPosition> _checkingPieces;

        public CheckedStateManager(BoardState boardState, KingMoveFilter kingMoveFilter)
        {
            _boardState = boardState;
            _kingMoveFilter = kingMoveFilter;
        }

        public bool IsCheck { get; private set; }


        /// <summary>
        ///     Checks for check
        ///     Updates private fields with checking pieces used to to determine multiple or single check, public property
        ///     isCheck
        /// </summary>
        /// <param name="nonTurnMoves"></param>
        /// <param name="kingPosition"></param>
        public void EvaluateCheck(IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            var checkingPieces = GetCheckingPieces(nonTurnMoves, kingPosition).ToList();
            _checkingPieces = checkingPieces;
            IsCheck = checkingPieces.Any();
        }

        /// <summary>
        ///     Updates turn move dictionary with the possible moves available under check
        /// </summary>
        /// <param name="turnMoves"></param>
        /// <param name="nonTurnMoves"></param>
        /// <param name="kingPosition"></param>
        /// <returns></returns>
        public IDictionary<BoardPosition, HashSet<BoardPosition>> UpdatePossibleMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            var checkedWithSinglePiece = _checkingPieces.Count() == 1;
            var checkedWithMultiplePieces = _checkingPieces.Count() > 1;

            if (checkedWithSinglePiece) UpdateWithInterceptingMoves(turnMoves, nonTurnMoves, kingPosition);
            if (checkedWithMultiplePieces) RemoveAllNonKingMoves(turnMoves, kingPosition);
            RemoveNonTurnMovesFromKingMoves(turnMoves, nonTurnMoves, kingPosition);
            return turnMoves;
        }

        private void UpdateWithInterceptingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            var positionsBetweenKingAndCheckPiece = GetPositionsBetweenCheckedKing(kingPosition, nonTurnMoves);
            foreach (var keyVal in turnMoves)
            {
                var notKingPiece = !keyVal.Key.Equals(kingPosition);
                if (notKingPiece) keyVal.Value.IntersectWith(positionsBetweenKingAndCheckPiece);
            }
        }

        private void RemoveNonTurnMovesFromKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            // this won't remove the moves of scanning pieces past king 
            _kingMoveFilter.RemoveNonTurnMovesFromKingMoves(turnMoves, nonTurnMoves, kingPosition, _boardState);

            // extra logic needed here to do that
            foreach (var checkingBoardPosition in _checkingPieces)
                if (ScanningPieces.Contains(GetPieceAt(checkingBoardPosition).Type))
                {
                    // remove extended possible moves that go 'through' king
                    var movesExtendedThroughKing =
                        checkingBoardPosition.Scan(checkingBoardPosition.DirectionTo(kingPosition));
                    var kingMoves = turnMoves[kingPosition];
                    turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(movesExtendedThroughKing));
                }
        }


        private IEnumerable<BoardPosition> GetCheckingPieces(
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves, BoardPosition kingPosition)
        {
            return nonTurnMoves
                .Where(nonTurnMove => nonTurnMove.Value.Contains(kingPosition))
                .Select(nonTurnMove => nonTurnMove.Key);
        }

        private HashSet<BoardPosition> GetPositionsBetweenCheckedKing(BoardPosition kingPosition,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves)
        {
            var checkingPiecePosition = _checkingPieces.First();
            var possibleMoves = nonTurnMoves[checkingPiecePosition];
            foreach (var boardPosition in possibleMoves)
                if (kingPosition.Equals(boardPosition))
                {
                    var result = ScanPositionGenerator.GetPositionsBetween(boardPosition, checkingPiecePosition)
                        .Concat(new List<BoardPosition> {checkingPiecePosition});
                    return new HashSet<BoardPosition>(result);
                }

            return new HashSet<BoardPosition>();
        }

        private static void RemoveAllNonKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            BoardPosition kingPosition)
        {
            foreach (var turnMove in turnMoves)
                if (!turnMove.Key.Equals(kingPosition))
                    turnMove.Value.Clear();
        }

        private State.PieceState.Piece GetCheckingPiece()
        {
            var checkingBoardPosition = _checkingPieces.First();
            return _boardState.Board[checkingBoardPosition.X, checkingBoardPosition.Y].CurrentPiece;
        }

        private State.PieceState.Piece GetPieceAt(BoardPosition boardPosition)
        {
            return _boardState.Board[boardPosition.X, boardPosition.Y].CurrentPiece;
        }
    }
}