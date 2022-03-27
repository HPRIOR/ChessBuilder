using System;
using System.Collections.Generic;
using Models.Services.Build.Interfaces;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Game.Implementations
{
    public sealed class GameInitializer
    {
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IPieceMoveGenerator _whiteKingMoveGenerator;

        private GameInitializer(IMovesGeneratorRepository movesGeneratorRepository,
            IBuildMoveGenerator buildMoveGenerator)
        {
            _buildMoveGenerator = buildMoveGenerator;
            _whiteKingMoveGenerator =
                movesGeneratorRepository.GetPossibleMoveGenerator(PieceType.WhiteKing, true);
        }

        public GameState InitialiseGame(BoardState boardState)
        {
            var whiteKingPosition = GetWhiteKingPosition(boardState);

            var moves = _whiteKingMoveGenerator.GetPossiblePieceMoves(whiteKingPosition, boardState);
            var movesDict = new Dictionary<Position, List<Position>>
            {
                { whiteKingPosition, new List<Position>(moves) }
            };

            var builds = _buildMoveGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(39));


            return new GameState(false, false, new PlayerState(39), movesDict, builds,
                boardState);
        }

        private Position GetWhiteKingPosition(BoardState boardState)
        {
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
                if (boardState.GetTileAt(i, j).CurrentPiece == PieceType.WhiteKing)
                    return new Position(i, j);
            throw new Exception("No white king found during game initialisation");
        }
    }
}