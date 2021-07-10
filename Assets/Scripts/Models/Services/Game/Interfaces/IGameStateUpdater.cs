using Models.State;
using Models.State.Board;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateUpdater
    {
        GameState UpdateGameState(BoardState newBoardState);
    }
}