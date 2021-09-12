using System.Collections.Generic;
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
            PlayerState = previousGameState.PlayerState;
            PossiblePieceMoves = previousGameState.PossiblePieceMoves; // may need to new this up
            BuildMoves = previousGameState.PossibleBuildMoves; // may need to new this up 
            Check = previousGameState.Check;
        }

        // BoardState Changes
        public Move Move { get; set; }
        public Build Build { get; set; }
        public IEnumerable<(Position, PieceType)> ResolvedBuilds { get; set; }
        public IEnumerable<Position> DecrementedTiles { get; set; }

        // BoardState History
        public PlayerState.PlayerState PlayerState { get; set; }
        public bool Check { get; set; }
        public BuildMoves BuildMoves { get; set; }
        public IDictionary<Position, HashSet<Position>> PossiblePieceMoves { get; set; }
    }
}