using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.Interfaces;
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

        // could possibly make this data static and return 
        public IDictionary<Position, HashSet<Position>> TurnMoves { get; private set; }
        public IDictionary<Position, HashSet<Position>> EnemyMoves { get; private set; }
        public Position KingPosition { get; private set; } = new Position(8, 8);

        public void EvaluateBoard(BoardState boardState, PieceColour turn)
        {
            var turnMoves = new Dictionary<Position, HashSet<Position>>();
            var enemyMoves = new Dictionary<Position, HashSet<Position>>();

            var activeTiles = boardState.ActivePieces.Select(position => boardState.Board[position.X, position.Y]);
            foreach (var tile in activeTiles)
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