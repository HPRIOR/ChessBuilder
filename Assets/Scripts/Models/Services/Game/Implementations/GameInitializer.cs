using System.Collections.Generic;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Game.Implementations
{
    public class GameInitializer
    {
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IPieceMoveGenerator _whiteKingMoveGenerator;

        private GameInitializer(IMovesGeneratorRepository movesGeneratorRepository,
            IBuildMoveGenerator buildMoveGenerator)
        {
            _buildMoveGenerator = buildMoveGenerator;
            _whiteKingMoveGenerator =
                movesGeneratorRepository.GetPossibleMoveGenerator(new Piece(PieceType.WhiteKing), true);
        }

        public GameState InitialiseGame(BoardState boardState)
        {
            var whiteKingPosition =
            (
                from Tile tile in boardState.Board
                where tile.CurrentPiece.Type.Equals(PieceType.WhiteKing)
                select tile.Position
            ).First();

            var moves = _whiteKingMoveGenerator.GetPossiblePieceMoves(whiteKingPosition, boardState);
            var movesDict = new Dictionary<Position, List<Position>>
            {
                { whiteKingPosition, new List<Position>(moves) }
            };

            var builds = _buildMoveGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(39));


            return new GameState(false, false, new PlayerState(39), movesDict, builds,
                boardState);
        }
    }
}