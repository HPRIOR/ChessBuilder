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

    public class AiMove : IEquatable<AiMove>
    {
        public AiMove(MoveType moveType, Position from, Position to,
            PieceType type)
        {
            MoveType = moveType;
            From = from;
            To = to;
            Type = type;
        }

        public MoveType MoveType { get; }
        public Position From { get; }
        public Position To { get; } // null if build move 
        public PieceType Type { get; } // null if build move 


        public bool Equals(AiMove other) => MoveType == other.MoveType && From.Equals(other.From) &&
                                            To.Equals(other.To) && Type == other.Type;

        public override bool Equals(object obj) => obj is AiMove other && Equals(other);

        public override int GetHashCode() => (From, To, Type).GetHashCode();

        public static bool operator ==(AiMove left, AiMove right) => left.Equals(right);

        public static bool operator !=(AiMove left, AiMove right) => !left.Equals(right);
    }
}