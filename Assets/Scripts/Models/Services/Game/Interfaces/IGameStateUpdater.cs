using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState GameState { get; }
        void UpdateGameState(BoardState newBoardState, PieceColour turn);
        void UpdateGameState(GameState previousGameState, Position from, Position to, PieceColour turn);

        void UpdateGameState(GameState previousGameState, Position buildPosition, PieceType piece,
            PieceColour turn);
    }
}