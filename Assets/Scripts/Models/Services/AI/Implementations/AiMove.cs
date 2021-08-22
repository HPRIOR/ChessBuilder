using System;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public enum MoveType
    {
        Build,
        Move
    }

    public class AiMove
    {
        public AiMove(MoveType moveType, Position from, Position to, Action<PieceColour> move)
        {
            MoveType = moveType;
            From = from;
            To = to;
            Move = move;
        }

        public MoveType MoveType { get; }
        public Position From { get; }
        public Position To { get; } // null if build move 
        public Action<PieceColour> Move { get; }
    }
}