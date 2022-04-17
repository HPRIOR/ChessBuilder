using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState UpdateGameState(Position from, Position to, PieceColour turn);

        GameState UpdateGameState(Position buildPosition, PieceType piece,
            PieceColour turn);

        GameState RevertGameState();

        void SetInitialGameState(GameState gameState);
    }
}