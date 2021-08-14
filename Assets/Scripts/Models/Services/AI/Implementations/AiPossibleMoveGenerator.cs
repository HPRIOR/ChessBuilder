// using System;
// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Linq;
// using Models.Services.AI.Interfaces;
// using Models.Services.Build.Interfaces;
// using Models.Services.Game.Interfaces;
// using Models.Services.Moves.Interfaces;
// using Models.State.Board;
// using Models.State.BuildState;
// using Models.State.GameState;
// using Models.State.PieceState;
// using Models.Utils.ExtensionMethods.PieceType;
//
// namespace Models.Services.AI.Implementations
// {
//     public class AiPossibleMoveGenerator : IAiPossibleMoveGenerator
//     {
//         private readonly IBuilder _builder;
//         private readonly IGameStateUpdater _gameStateUpdater;
//         private readonly IPieceMover _pieceMover;
//
//         public AiPossibleMoveGenerator(IPieceMover pieceMover, IGameStateUpdater gameStateUpdater, IBuilder builder)
//         {
//             _pieceMover = pieceMover;
//             _gameStateUpdater = gameStateUpdater;
//             _builder = builder;
//         }
//
//         public IEnumerable<Func<GameState, PieceColour, GameState>> GenerateMoves(GameState gameState) =>
//             GetBuildCommands(gameState.PossibleBuildMoves)
//                 .Concat(GetMoveCommands(gameState.PossiblePieceMoves));
//
//         private IEnumerable<Func<GameState, PieceColour, GameState>> GetMoveCommands(
//             ImmutableDictionary<Position, ImmutableHashSet<Position>> moves
//         ) =>
//             moves.SelectMany(moveSet =>
//                 moveSet.Value.Select(move =>
//                 {
//                     Func<GameState, PieceColour, GameState> command = (previousGameState, turn) =>
//                         _gameStateUpdater.UpdateGameState(previousGameState, moveSet.Key, move, turn.NextTurn());
//                     return command;
//                 }));
//
//
//         private IEnumerable<Func<GameState, PieceColour, GameState>> GetBuildCommands(BuildMoves builds) =>
//             builds.BuildPositions.SelectMany(position =>
//                 builds.BuildPieces.Select(piece =>
//                 {
//                     Func<GameState, PieceColour, GameState> command = (previousGameState, turn) =>
//                         _gameStateUpdater.UpdateGameState(previousGameState, position, piece, turn.NextTurn());
//                     return command;
//                 }));
//     }
// }

