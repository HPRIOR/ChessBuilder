using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateController
    {
        GameState CurrentGameState { get; }
        PieceColour Turn { get; }
        void UpdateGameState(BoardState newBoardState);
        void UpdateGameState(GameState newGameState);
        void InitializeGame(BoardState boardState);
        void RetainBoardState();
    }
}