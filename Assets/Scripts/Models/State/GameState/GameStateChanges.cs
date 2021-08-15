using System.Collections.Generic;
using System.Collections.Immutable;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.GameState
{
    public class Move
    {
        public Move(Position to, Position from)
        {
            To = to;
            From = from;
        }

        public Position To { get; }
        public Position From { get; }
    }

    public class Build
    {
        public Build(Position at, PieceType piece)
        {
            At = at;
            Piece = piece;
        }

        public Position At { get; }
        public PieceType Piece { get; }
    }


    /// <summary>
    ///     This contains information regarding the previous state, and also the changes that occured to get to the next state
    ///     For example, the available moves are stored from the previous state, whereas the change in BoardState is recorded
    ///     so that it can be reverted.
    /// </summary>
    public class GameStateChanges
    {
        public GameStateChanges()
        {
        }

        public GameStateChanges(GameState previousGameState)
        {
            BlackPlayerState = previousGameState.BlackState;
            WhitePlayerState = previousGameState.WhiteState;
            PossiblePieceMoves = previousGameState.PossiblePieceMoves;
            BuildMoves = previousGameState.PossibleBuildMoves;
            Check = previousGameState.Check;
        }

        // BoardState Changes
        public Move Move { get; set; }
        public Build Build { get; set; }
        public IEnumerable<(Position, PieceType)> ResolvedBuilds { get; set; }

        // BoardState History
        public PlayerState.PlayerState WhitePlayerState { get; set; }
        public PlayerState.PlayerState BlackPlayerState { get; set; }
        public bool Check { get; set; }
        public BuildMoves BuildMoves { get; set; }
        public ImmutableDictionary<Position, ImmutableHashSet<Position>> PossiblePieceMoves { get; set; }
        public IEnumerable<Position> DecrementedTiles { get; set; }
        public PieceColour Turn { get; set; }
    }
}