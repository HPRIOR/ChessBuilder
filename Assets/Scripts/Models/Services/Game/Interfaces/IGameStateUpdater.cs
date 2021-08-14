﻿using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState GameState { get; }
        void UpdateGameState(BoardState boardState, PieceColour turn);
        void UpdateGameState(Position from, Position to, PieceColour turn);

        void UpdateGameState(Position buildPosition, PieceType piece,
            PieceColour turn);
    }
}