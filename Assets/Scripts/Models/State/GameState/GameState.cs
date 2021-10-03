using System;
using System.Collections.Generic;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.GameState
{
    public class GameState : ICloneable
    {
        public GameState(bool check, bool checkMate,
            PlayerState.PlayerState playerState,
            IDictionary<Position, List<Position>> possiblePieceMoves,
            BuildMoves possibleBuildMoves, BoardState boardState)
        {
            Check = check;
            CheckMate = checkMate;
            PlayerState = playerState;
            PossiblePieceMoves = possiblePieceMoves;
            PossibleBuildMoves = possibleBuildMoves;
            BoardState = boardState;
        }

        public BoardState BoardState { get; set; }
        public bool Check { get; set; }
        public bool CheckMate { get; set; }
        public PlayerState.PlayerState PlayerState { get; set; }
        public IDictionary<Position, List<Position>> PossiblePieceMoves { get; set; }
        public BuildMoves PossibleBuildMoves { get; set; }

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