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
        public IDictionary<Position, List<Position>> TurnMoves { get; private set; }
        public IDictionary<Position, List<Position>> EnemyMoves { get; private set; }
        public Position KingPosition { get; private set; } = new Position(8, 8);

        public void EvaluateBoard(BoardState boardState, PieceColour turn)
        {
            var turnMoves = new Dictionary<Position, List<Position>>();
            var enemyMoves = new Dictionary<Position, List<Position>>();

            var activeTiles = boardState.ActivePieces.ToArray()
                .Select(position => boardState.Board[position.X][position.Y]);

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

                    // converting to hashset here is inefficient!
                    turnMoves.Add(boardPos, possibleMoves);
                }

                if (opponentTurn)
                {
                    var boardPos = tile.Position;
                    var possibleMoves = _movesGeneratorRepository.GetPossibleMoveGenerator(currentPiece, false)
                        .GetPossiblePieceMoves(boardPos, boardState);
                    // converting to hashset here is inefficient!
                    enemyMoves.Add(boardPos, possibleMoves);
                }

                TurnMoves = turnMoves;
                EnemyMoves = enemyMoves;
            }
        }
    }
}