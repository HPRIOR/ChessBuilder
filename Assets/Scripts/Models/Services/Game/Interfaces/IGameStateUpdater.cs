using Models.State.Board;
using Models.State.GameState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState UpdateGameState(BoardState newBoardState);
    }
}