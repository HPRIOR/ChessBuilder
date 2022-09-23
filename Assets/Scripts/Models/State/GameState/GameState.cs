using System;
using System.Collections.Generic;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.GameState
{
    public sealed class GameState : ICloneable
    {
        public GameState(
            bool check,
            bool checkMate,
            PlayerState.PlayerState playerState,
            Dictionary<Position, List<Position>> possiblePieceMoves,
            BuildMoves possibleBuildMoves,
            BoardState boardState)
        {
            Check = check;
            CheckMate = checkMate;
            PlayerState = playerState;
            PossiblePieceMoves = possiblePieceMoves;
            PossibleBuildMoves = possibleBuildMoves;
            BoardState = boardState;
        }

        public BoardState BoardState { get; }
        public bool Check { get; }
        public bool CheckMate { get; }
        public PlayerState.PlayerState PlayerState { get; }
        public IDictionary<Position, List<Position>> PossiblePieceMoves { get; }
        public BuildMoves PossibleBuildMoves { get; }

        public object Clone()
        {
            var possibleMoves = new Dictionary<Position, List<Position>>(PossiblePieceMoves);
            var builds = new List<PieceType>(PossibleBuildMoves.BuildPieces);
            var buildPosition = new List<Position>(PossibleBuildMoves.BuildPositions);
            var buildMoves = new BuildMoves(buildPosition, builds);
            return new GameState(Check, CheckMate, PlayerState, possibleMoves, buildMoves,
                BoardState.Clone());
        }
    }
}