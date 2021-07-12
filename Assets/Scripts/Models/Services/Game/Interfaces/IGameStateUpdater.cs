using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState UpdateGameState(BoardState newBoardState, PieceColour turn);
    }
}