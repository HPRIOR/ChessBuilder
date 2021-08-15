using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState GameState { get; }
        void UpdateGameState(PieceColour turn);
        void RevertGameState();
        GameStateChanges UpdateGameState(Position from, Position to, PieceColour turn);

        GameStateChanges UpdateGameState(Position buildPosition, PieceType piece,
            PieceColour turn);

        void RevertGameStateChanges(GameStateChanges gameStateChanges);
    }
}