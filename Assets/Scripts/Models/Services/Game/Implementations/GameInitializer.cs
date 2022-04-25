using System.Collections.Generic;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;
using Models.State.PlayerState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Game.Implementations
{
    public sealed class GameInitializer
    {
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IMovesGeneratorRepository _movesGeneratorRepository;

        private GameInitializer(IMovesGeneratorRepository movesGeneratorRepository,
            IBuildMoveGenerator buildMoveGenerator)
        {
            _movesGeneratorRepository = movesGeneratorRepository;
            _buildMoveGenerator = buildMoveGenerator;
        }

        private Dictionary<PieceType, IPieceMoveGenerator> GetPieceMoveGenerators(BoardState boardState) =>
            boardState.ActivePieces
                .Select(position => boardState.GetTileAt(position))
                .Where(tile => tile.CurrentPiece.Colour() == PieceColour.White)
                .Select(tile => tile.CurrentPiece).Distinct()
                .ToDictionary(piece => piece,
                    piece => _movesGeneratorRepository.GetPossibleMoveGenerator(piece, true));

        private List<(Position pos, PieceType type)> GetPiecePositions(BoardState boardState) =>
            boardState.ActivePieces
                .Select(position => boardState.GetTileAt(position))
                .Where(tile => tile.CurrentPiece.Colour() == PieceColour.White)
                .Select(tile => (position: tile.Position, boardState.GetTileAt(tile.Position).CurrentPiece))
                .ToList();

        public GameState InitialiseGame(BoardState boardState)
        {
            var pieceMoveGenerators = GetPieceMoveGenerators(boardState);
            var piecePositions = GetPiecePositions(boardState);

            var movesDict =
                piecePositions.ToDictionary(
                    piecePos => piecePos.pos,
                    piecePos => pieceMoveGenerators[piecePos.type]
                        .GetPossiblePieceMoves(piecePos.pos, boardState));

            var whitePieceValue =
                piecePositions
                    .Where(pp => pp.type.Colour() == PieceColour.White)
                    .Select(pp => pp.type.Value()).Sum();

            // todo inject total build points
            var buildPoints = 39 - whitePieceValue;


            var builds =
                _buildMoveGenerator.GetPossibleBuildMoves(boardState, PieceColour.White, new PlayerState(buildPoints));
            return new GameState(false, false, new PlayerState(buildPoints), movesDict, builds, boardState);
        }
    }
}