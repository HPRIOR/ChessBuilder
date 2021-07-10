using System.Collections.Immutable;
using Models.State.Board;
using Models.State.BuildState;

namespace Models.State.GameState
{
    public class GameState
    {
        public GameState(bool check, bool checkMate, PlayerState.PlayerState blackState,
            PlayerState.PlayerState whiteState,
            ImmutableDictionary<Position, ImmutableHashSet<Position>> possiblePieceMoves,
            BuildMoves possibleBuildMoves)
        {
            Check = check;
            CheckMate = checkMate;
            BlackState = blackState;
            WhiteState = whiteState;
            PossiblePieceMoves = possiblePieceMoves;
            PossibleBuildMoves = possibleBuildMoves;
        }

        public bool Check { get; }
        public bool CheckMate { get; }
        public PlayerState.PlayerState BlackState { get; }
        public PlayerState.PlayerState WhiteState { get; }
        public ImmutableDictionary<Position, ImmutableHashSet<Position>> PossiblePieceMoves { get; }
        public BuildMoves PossibleBuildMoves { get; }
    }
}