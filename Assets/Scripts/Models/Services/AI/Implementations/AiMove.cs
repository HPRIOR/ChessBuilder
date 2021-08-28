using System;
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

    public readonly struct AiMove
    {
        public AiMove(MoveType moveType, Position from, Position to, Action<PieceColour, IGameStateUpdater> move)
        {
            MoveType = moveType;
            From = from;
            To = to;
            Move = move;
        }

        public MoveType MoveType { get; }
        public Position From { get; }
        public Position To { get; } // null if build move 
        public Action<PieceColour, IGameStateUpdater> Move { get; }
    }
}