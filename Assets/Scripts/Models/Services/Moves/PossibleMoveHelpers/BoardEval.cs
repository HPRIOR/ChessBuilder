using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.PossibleMoveHelpers
{
    public class BoardEval : IBoardEval
    {
        private readonly IMoveGeneratorRepository _moveGeneratorRepository;

        public BoardEval(IMoveGeneratorRepository moveGeneratorRepository)
        {
            _moveGeneratorRepository = moveGeneratorRepository;
        }

        public IDictionary<BoardPosition, HashSet<BoardPosition>> TurnMoves { get; private set; }
        public IDictionary<BoardPosition, HashSet<BoardPosition>> NonTurnMoves { get; private set; }
        public BoardPosition KingPosition { get; private set; } = new BoardPosition(8, 8);

        // TODO refactor
        public void EvaluateBoard(BoardState boardState, PieceColour turn)
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
                        KingPosition = tile.BoardPosition;
                    var boardPos = tile.BoardPosition;
                    var possibleMoves = _moveGeneratorRepository.GetPossibleMoveGenerator(currentPiece, true)
                        .GetPossiblePieceMoves(boardPos, boardState);

                    turnMoves.Add(boardPos, new HashSet<BoardPosition>(possibleMoves));
                }

                if (notPiecesTurn)
                {
                    var boardPos = tile.BoardPosition;
                    var possibleMoves = _moveGeneratorRepository.GetPossibleMoveGenerator(currentPiece, true)
                        .GetPossiblePieceMoves(boardPos, boardState);
                    nonTurnMoves.Add(boardPos, new HashSet<BoardPosition>(possibleMoves));
                }

                TurnMoves = turnMoves;
                NonTurnMoves = nonTurnMoves;
            }
        }
    }
}