using System.Collections.Generic;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState GameState { get; }
        Stack<GameStateChanges> StateHistory { get; }
        void RevertGameState();
        void UpdateGameState(Position from, Position to, PieceColour turn);

        void UpdateGameState(Position buildPosition, PieceType piece,
            PieceColour turn);
    }
}