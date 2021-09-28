using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.AI.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.AI.Implementations
{
    public class AiPossibleMoveGenerator : IAiPossibleMoveGenerator
    {
        public IEnumerable<AiMove> GenerateMoves(GameState gameState) =>
            GetMoveCommands(gameState.PossiblePieceMoves);
        // GetBuildCommands(gameState.PossibleBuildMoves)
        // .Concat(GetMoveCommands(gameState.PossiblePieceMoves));

        private IEnumerable<AiMove> GetMoveCommands(IDictionary<Position, List<Position>> moves)
        {
            var result = new List<AiMove>();
            foreach (var moveSet in moves)
            {
                foreach (var move in moveSet.Value)
                {
                    Action<PieceColour, IGameStateUpdater> command = (turn, gameStateUpdater) =>
                        gameStateUpdater.UpdateGameState(moveSet.Key, move, turn.NextTurn());
                    result.Add(
                            new AiMove(MoveType.Move, moveSet.Key, move, command, PieceType.NullPiece)
                        );
                }
            }

            return result;

            // return moves.SelectMany(moveSet =>
            //     moveSet.Value.Select(move =>
            //     {
            //         Action<PieceColour, IGameStateUpdater> command = (turn, gameStateUpdater) =>
            //             gameStateUpdater.UpdateGameState(moveSet.Key, move, turn.NextTurn());
            //         return new AiMove(MoveType.Move, moveSet.Key, move, command, PieceType.NullPiece);
            //     }));
        }


        private IEnumerable<AiMove> GetBuildCommands(BuildMoves builds) =>
            builds.BuildPositions.SelectMany(position =>
                builds.BuildPieces.Select(piece =>
                {
                    Action<PieceColour, IGameStateUpdater> command = (turn, gameStateUpdater) =>
                        gameStateUpdater.UpdateGameState(position, piece, turn.NextTurn());
                    return new AiMove(MoveType.Build, position, new Position(), command, piece);
                }));
    }
}