using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState UpdateGameState(BoardState newBoardState, PieceColour turn);
        GameState UpdateGameState(GameState previousGameState, Position from, Position to, PieceColour turn);

        GameState UpdateGameState(GameState previousGameState, Position buildPosition, PieceType piece,
            PieceColour turn);
    }
}