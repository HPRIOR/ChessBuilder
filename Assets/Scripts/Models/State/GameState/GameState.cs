using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.GameState
{
    public class GameState : ICloneable
    {
        public GameState(bool check, bool checkMate, PlayerState.PlayerState blackState,
            PlayerState.PlayerState whiteState,
            ImmutableDictionary<Position, ImmutableHashSet<Position>> possiblePieceMoves,
            BuildMoves possibleBuildMoves, BoardState boardState)
        {
            Check = check;
            CheckMate = checkMate;
            BlackState = blackState;
            WhiteState = whiteState;
            PossiblePieceMoves = possiblePieceMoves;
            PossibleBuildMoves = possibleBuildMoves;
            BoardState = boardState;
        }

        public BoardState BoardState { get; set; }
        public bool Check { get; set; }
        public bool CheckMate { get; set; }
        public PlayerState.PlayerState BlackState { get; set; }
        public PlayerState.PlayerState WhiteState { get; set; }
        public ImmutableDictionary<Position, ImmutableHashSet<Position>> PossiblePieceMoves { get; set; }
        public BuildMoves PossibleBuildMoves { get; set; }

        public object Clone()
        {
            var possibleMoves = new Dictionary<Position, ImmutableHashSet<Position>>(PossiblePieceMoves)
                .ToImmutableDictionary();
            var builds = new HashSet<PieceType>(PossibleBuildMoves.BuildPieces).ToImmutableHashSet();
            var buildPosition = new HashSet<Position>(PossibleBuildMoves.BuildPositions).ToImmutableHashSet();
            var buildMoves = new BuildMoves(buildPosition, builds);
            return new GameState(Check, CheckMate, BlackState, WhiteState, possibleMoves, buildMoves,
                BoardState.Clone());
        }
    }
}