﻿using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateController
    {
        GameState CurrentGameState { get; }
        PieceColour Turn { get; }
        void UpdateGameState(BoardState newBoardState);
        void RevertGameState();
        void UpdateGameState(Position buildPiece, PieceType piece);
        void UpdateGameState(Position from, Position to);
        void InitializeGame(BoardState boardState);
        void RetainBoardState();
    }
}