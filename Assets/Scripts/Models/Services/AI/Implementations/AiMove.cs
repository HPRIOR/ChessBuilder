﻿using System;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public enum MoveType
    {
        Build,
        Move
    }

    public readonly struct AiMove : IEquatable<AiMove>
    {
        public AiMove(MoveType moveType, Position from, Position to, Action<PieceColour, IGameStateUpdater> move,
            PieceType type)
        {
            MoveType = moveType;
            From = from;
            To = to;
            Move = move;
            Type = type;
        }

        public MoveType MoveType { get; }
        public Position From { get; }
        public Position To { get; } // null if build move 
        public PieceType Type { get; } // null if build move 
        public Action<PieceColour, IGameStateUpdater> Move { get; }


        public bool Equals(AiMove other) => MoveType == other.MoveType && From.Equals(other.From) &&
                                            To.Equals(other.To) && Type == other.Type;

        public override bool Equals(object obj) => obj is AiMove other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)MoveType;
                hashCode = (hashCode * 397) ^ From.GetHashCode();
                hashCode = (hashCode * 397) ^ To.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Type;
                return hashCode;
            }
        }

        public static bool operator ==(AiMove left, AiMove right) => left.Equals(right);

        public static bool operator !=(AiMove left, AiMove right) => !left.Equals(right);
    }
}