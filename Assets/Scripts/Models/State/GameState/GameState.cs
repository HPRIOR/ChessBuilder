using System;
using System.Collections.Immutable;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State
{
    public class GameState : ICloneable
    {
        public GameState(PieceColour turn, bool check, bool checkMate, PlayerState.PlayerState blackState,
            PlayerState.PlayerState whiteState,
            ImmutableDictionary<Position, ImmutableHashSet<Position>> possiblePieceMoves,
            BuildMoves possibleBuildMoves)
        {
            Turn = turn;
            Check = check;
            CheckMate = checkMate;
            BlackState = blackState;
            WhiteState = whiteState;
            PossiblePieceMoves = possiblePieceMoves;
            PossibleBuildMoves = possibleBuildMoves;
        }

        public PieceColour Turn { get; }
        public bool Check { get; }
        public bool CheckMate { get; }
        public PlayerState.PlayerState BlackState { get; }
        public PlayerState.PlayerState WhiteState { get; }
        public ImmutableDictionary<Position, ImmutableHashSet<Position>> PossiblePieceMoves { get; }
        public BuildMoves PossibleBuildMoves { get; }

        public object Clone() => new GameState(Turn, Check, CheckMate, BlackState, WhiteState,
            PossiblePieceMoves, PossibleBuildMoves);
    }
}