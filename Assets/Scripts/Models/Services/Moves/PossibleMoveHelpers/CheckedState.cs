using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.BoardPos;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class CheckedState : ICheckedState
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
        private readonly IPossibleMoveFactory _possibleMoveFactory;
        private readonly BoardPosition _previousMove;
        private IEnumerable<BoardPosition> _checkingPieces;

        private HashSet<BoardPosition> _positionsBetweenKingAndCheckPiece;


        // TODO 1. inject possible move factory to this; 2. Check all non-turn moves for check; 3. Inject into PossibleMoveGen;
        public CheckedState(BoardState boardState,
            BoardPosition previousMove,
            IPossibleMoveFactory possibleMoveFactory)
        {
            _boardState = boardState;
            _previousMove = previousMove;
            _possibleMoveFactory = possibleMoveFactory;
        }

        public bool IsTrue { get; private set; }


        public void EvaluateCheck(IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            var checkingPieces = GetCheckingPieces(nonTurnMoves, kingPosition).ToList();
            if (checkingPieces.Count == 1)
                _positionsBetweenKingAndCheckPiece = GetPositionsBetweenCheckedKing(kingPosition);
            IsTrue = checkingPieces.Any();
            _checkingPieces = checkingPieces;
        }

        public IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            if (_checkingPieces.Count() == 1) turnMoves = PossibleNonKingMovesWhenInCheck(turnMoves, kingPosition);
            if (_checkingPieces.Count() > 1) turnMoves = RemoveAllNonKingMoves(turnMoves, kingPosition);
            turnMoves = PossibleKingMovesWhenInCheck(turnMoves, nonTurnMoves, kingPosition);
            return turnMoves;
        }

        private IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleNonKingMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            BoardPosition kingPosition)
        {
            foreach (var keyVal in turnMoves)
            {
                var notKingPiece = !keyVal.Key.Equals(kingPosition);
                if (notKingPiece) keyVal.Value.IntersectWith(_positionsBetweenKingAndCheckPiece);
            }

            return turnMoves;
        }

        private IDictionary<BoardPosition, HashSet<BoardPosition>> PossibleKingMovesWhenInCheck(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            BoardPosition kingPosition)
        {
            var checkingPiece = _boardState.Board[_previousMove.X, _previousMove.Y].CurrentPiece;
            RemoveNonTurnMoves(turnMoves, nonTurnMoves, kingPosition);
            // check if checking piece is scanning type 
            if (ScanningPieces.Contains(checkingPiece.Type))
            {
                // remove extended 
                var movesExtendedThroughKing = _previousMove.Scan(_previousMove.DirectionTo(kingPosition));
                var kingMoves = turnMoves[kingPosition];
                turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(movesExtendedThroughKing));
            }

            return turnMoves;
        }

        private IEnumerable<BoardPosition> GetCheckingPieces(
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves, BoardPosition kingPosition)
        {
            return nonTurnMoves
                .Where(nonTurnMove => nonTurnMove.Value.Contains(kingPosition))
                .Select(nonTurnMove => nonTurnMove.Key);
        }

        private static void RemoveNonTurnMoves(IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves, BoardPosition kingPosition)
        {
            // if not, remove all possible moves from king moves
            foreach (var nonTurnMove in nonTurnMoves)
            {
                var kingMoves = turnMoves[kingPosition];
                turnMoves[kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(nonTurnMove.Value));
            }
        }

        private HashSet<BoardPosition> GetPositionsBetweenCheckedKing(BoardPosition kingPosition)
        {
            var movedPiece = _boardState.Board[_previousMove.X, _previousMove.Y].CurrentPiece;
            var possibleMoves =
                _possibleMoveFactory
                    .GetPossibleMoveGenerator(movedPiece)
                    .GetPossiblePieceMoves(_previousMove, _boardState);

            foreach (var boardPosition in possibleMoves)
                if (kingPosition.Equals(boardPosition))
                {
                    var result = ScanPositionGenerator.GetPositionsBetween(boardPosition, _previousMove)
                        .Concat(new List<BoardPosition> {_previousMove});
                    return new HashSet<BoardPosition>(result);
                }

            return new HashSet<BoardPosition>();
        }

        private IDictionary<BoardPosition, HashSet<BoardPosition>> RemoveAllNonKingMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            BoardPosition kingPosition)
        {
            foreach (var turnMove in turnMoves)
                if (!turnMove.Key.Equals(kingPosition))
                    turnMove.Value.Clear();
            return turnMoves;
        }
    }
}