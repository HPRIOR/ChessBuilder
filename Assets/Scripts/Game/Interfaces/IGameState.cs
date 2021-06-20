﻿using System.Collections.Generic;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Game.Interfaces
{
    public interface IGameState
    {
        PieceColour Turn { get; }
        BoardState CurrentBoardState { get; }
        public PlayerState BlackState { get; }
        public PlayerState WhiteState { get; }
        public IDictionary<Position, HashSet<PieceType>> PossibleBuildMoves { get; }
        IDictionary<Position, HashSet<Position>> PossiblePieceMoves { get; }
        void UpdateBoardState(BoardState newState);
        void RetainBoardState();
    }
}