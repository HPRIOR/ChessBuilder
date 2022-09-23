using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Moves.Utils
{
    public sealed class BoardInfo : IBoardInfo
    {
        private readonly IMovesGeneratorRepository _movesGeneratorRepository;

        public BoardInfo(IMovesGeneratorRepository movesGeneratorRepository)
        {
            _movesGeneratorRepository = movesGeneratorRepository;
        }

        // could possibly make this data static and return 
        public Dictionary<Position, List<Position>> TurnMoves { get; private set; }
        public Dictionary<Position, List<Position>> EnemyMoves { get; private set; }
        public Position KingPosition { get; private set; } = new(8, 8);

        public void EvaluateBoard(BoardState boardState, PieceColour turn)
        {
            var turnMoves = new Dictionary<Position, List<Position>>();
            var enemyMoves = new Dictionary<Position, List<Position>>();

            for (var index = 0; index < boardState.ActivePieces.Count; index++)
            {
                var pos = boardState.ActivePieces[index];
                var tile = boardState.GetTileAt(pos);
                var currentPiece = tile.CurrentPiece;
                var playerTurn = currentPiece != PieceType.NullPiece && currentPiece.Colour() == turn;
                var opponentTurn = currentPiece != PieceType.NullPiece && currentPiece.Colour() != turn;
                if (playerTurn)
                {
                    if (currentPiece == PieceType.BlackKing || currentPiece == PieceType.WhiteKing)
                        KingPosition = tile.Position;
                    var boardPos = tile.Position;
                    var possibleMoves = _movesGeneratorRepository.GetPossibleMoveGenerator(currentPiece, true)
                        .GetPossiblePieceMoves(boardPos, boardState);

                    turnMoves.Add(boardPos, possibleMoves);
                }

                if (opponentTurn)
                {
                    var boardPos = tile.Position;
                    var possibleMoves = _movesGeneratorRepository.GetPossibleMoveGenerator(currentPiece, false)
                        .GetPossiblePieceMoves(boardPos, boardState);
                    enemyMoves.Add(boardPos, possibleMoves);
                }

                TurnMoves = turnMoves;
                EnemyMoves = enemyMoves;
            }
        }
    }
}