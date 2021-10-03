using System.Collections.Generic;
using Models.Services.AI.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;

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
                for (var index = 0; index < moveSet.Value.Count; index++)
                {
                    var move = moveSet.Value[index];
                    result.Add(new AiMove(MoveType.Move, moveSet.Key, move, PieceType.NullPiece));
                }
            }

            return result;
        }


        private IEnumerable<AiMove> GetBuildCommands(BuildMoves builds)
        {
            var result = new List<AiMove>();
            foreach (var position in builds.BuildPositions)
            foreach (var piece in builds.BuildPieces)
                result.Add(new AiMove(MoveType.Build, position, new Position(), piece));
            return result;
        }
    }
}