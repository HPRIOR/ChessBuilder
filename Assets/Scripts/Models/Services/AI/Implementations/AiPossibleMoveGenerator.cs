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
            for (var index = 0; index < builds.BuildPositions.Count; index++)
            {
                var position = builds.BuildPositions[index];
                for (var i = 0; i < builds.BuildPieces.Count; i++)
                {
                    var piece = builds.BuildPieces[i];
                    result.Add(new AiMove(MoveType.Build, position, new Position(), piece));
                }
            }

            return result;
        }
    }
}