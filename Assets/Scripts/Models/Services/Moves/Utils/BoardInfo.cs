using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.Moves.Utils
{
    public class BoardInfo : IBoardInfo
    {
        private readonly IMovesGeneratorRepository _movesGeneratorRepository;

        public BoardInfo(IMovesGeneratorRepository movesGeneratorRepository)
        {
            _movesGeneratorRepository = movesGeneratorRepository;
        }

        public IDictionary<Position, HashSet<Position>> TurnMoves { get; private set; }
        public IDictionary<Position, HashSet<Position>> EnemyMoves { get; private set; }
        public Position KingPosition { get; private set; } = new Position(8, 8);

        public void EvaluateBoard(BoardState boardState, PieceColour turn)
        {
            var board = boardState.Board;
            var turnMoves = new Dictionary<Position, HashSet<Position>>();
            var enemyMoves = new Dictionary<Position, HashSet<Position>>();
            foreach (var tile in board)
            {
                var currentPiece = tile.CurrentPiece;
                var playerTurn = currentPiece.Type != PieceType.NullPiece && currentPiece.Colour == turn;
                var opponentTurn = currentPiece.Type != PieceType.NullPiece && currentPiece.Colour != turn;
                if (playerTurn)
                {
                    if (currentPiece.Type == PieceType.BlackKing || currentPiece.Type == PieceType.WhiteKing)
                        KingPosition = tile.Position;
                    var boardPos = tile.Position;
                    var possibleMoves = _movesGeneratorRepository.GetPossibleMoveGenerator(currentPiece, true)
                        .GetPossiblePieceMoves(boardPos, boardState);

                    turnMoves.Add(boardPos, new HashSet<Position>(possibleMoves));
                }

                if (opponentTurn)
                {
                    var boardPos = tile.Position;
                    var possibleMoves = _movesGeneratorRepository.GetPossibleMoveGenerator(currentPiece, false)
                        .GetPossiblePieceMoves(boardPos, boardState);
                    enemyMoves.Add(boardPos, new HashSet<Position>(possibleMoves));
                }

                TurnMoves = turnMoves;
                EnemyMoves = enemyMoves;
            }
        }
    }
}