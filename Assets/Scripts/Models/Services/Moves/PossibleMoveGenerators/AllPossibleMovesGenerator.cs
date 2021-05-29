using System.Collections.Generic;
using System.Linq;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveGenerators
{
    public class AllPossibleMovesGenerator : IAllPossibleMovesGenerator
    {
        private readonly IPossibleMoveFactory _possibleMoveFactory;
        private BoardPosition _kingPosition;

        public AllPossibleMovesGenerator(IPossibleMoveFactory possibleMoveFactory)
        {
            _possibleMoveFactory = possibleMoveFactory;
        }


        public IDictionary<BoardPosition, HashSet<BoardPosition>> GetPossibleMoves(BoardState boardState,
            PieceColour turn, BoardPosition previousMove)
        {
            var (turnMoves, nonTurnMoves) = GetTurnMoves(boardState, turn);
            var checkedState = new CheckedState(boardState, previousMove, turn, _possibleMoveFactory);
            if (checkedState.IsTrue)
                turnMoves =
                    (Dictionary<BoardPosition, HashSet<BoardPosition>>) checkedState.PossibleNonKingMovesWhenInCheck(
                        turnMoves);

            turnMoves = IntersectKingMovesWithNonTurnMoves(nonTurnMoves, turnMoves);
            return turnMoves;
        }

        private (IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves) GetTurnMoves(BoardState boardState,
                PieceColour turn)
        {
            var board = boardState.Board;
            var turnMoves = new Dictionary<BoardPosition, HashSet<BoardPosition>>();
            var nonTurnMoves = new Dictionary<BoardPosition, HashSet<BoardPosition>>();
            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                var piecesTurn = currentPiece.Type != PieceType.NullPiece && currentPiece.Colour == turn;
                var notPiecesTurn = currentPiece.Type != PieceType.NullPiece && currentPiece.Colour != turn;
                if (piecesTurn)
                {
                    if (currentPiece.Type == PieceType.BlackKing || currentPiece.Type == PieceType.WhiteKing)
                        _kingPosition = tile.BoardPosition;
                    var boardPos = tile.BoardPosition;
                    var possibleMoves = _possibleMoveFactory.GetPossibleMoveGenerator(currentPiece)
                        .GetPossiblePieceMoves(boardPos, boardState);

                    turnMoves.Add(boardPos, new HashSet<BoardPosition>(possibleMoves));
                }

                if (notPiecesTurn)
                {
                    var boardPos = tile.BoardPosition;
                    var possibleMoves = _possibleMoveFactory.GetPossibleMoveGenerator(currentPiece)
                        .GetPossiblePieceMoves(boardPos, boardState);
                    nonTurnMoves.Add(boardPos, new HashSet<BoardPosition>(possibleMoves));
                }
            }

            return (turnMoves, nonTurnMoves);
        }

        private IDictionary<BoardPosition, HashSet<BoardPosition>> IntersectKingMovesWithNonTurnMoves(
            IDictionary<BoardPosition, HashSet<BoardPosition>> nonTurnMoves,
            IDictionary<BoardPosition, HashSet<BoardPosition>> turnMoves)
        {
            foreach (var keyVal in nonTurnMoves)
            {
                var kingMoves = turnMoves[_kingPosition];
                turnMoves[_kingPosition] = new HashSet<BoardPosition>(kingMoves.Except(keyVal.Value));
            }

            return turnMoves;
        }
    }
}