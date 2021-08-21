using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Models.Services.AI.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;

namespace Models.Services.AI.Implementations
{
    public class AiPossibleMoveGenerator : IAiPossibleMoveGenerator
    {
        public IEnumerable<Action<PieceColour, IGameStateUpdater>> GenerateMoves(GameState gameState) =>
            GetBuildCommands(gameState.PossibleBuildMoves)
                .Concat(GetMoveCommands(gameState.PossiblePieceMoves));

        private IEnumerable<Action<PieceColour, IGameStateUpdater>> GetMoveCommands(
            ImmutableDictionary<Position, ImmutableHashSet<Position>> moves
        ) =>
            moves.SelectMany(moveSet =>
                moveSet.Value.Select(move =>
                {
                    Action<PieceColour, IGameStateUpdater> command = (turn, gameStateUpdater) =>
                        gameStateUpdater.UpdateGameState(moveSet.Key, move, turn.NextTurn());
                    return command;
                }));


        private IEnumerable<Action<PieceColour, IGameStateUpdater>> GetBuildCommands(BuildMoves builds) =>
            builds.BuildPositions.SelectMany(position =>
                builds.BuildPieces.Select(piece =>
                {
                    Action<PieceColour, IGameStateUpdater> command = (turn, gameStateUpdater) =>
                        gameStateUpdater.UpdateGameState(position, piece, turn.NextTurn());
                    return command;
                }));
    }
}