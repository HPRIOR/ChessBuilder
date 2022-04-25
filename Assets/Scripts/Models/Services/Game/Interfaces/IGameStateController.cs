using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameStateController
    {
        GameState CurrentGameState { get; }
        PieceColour Turn { get; }
        void RevertGameState();
        void UpdateGameState(Position buildPosition, PieceType piece);
        void UpdateGameState(Position from, Position to);
        void InitializeGame(BoardState boardState);
        void RetainBoardState();
        bool IsValidMove(Position buildPosition, PieceType piece);
        bool IsValidMove(Position from, Position to);
    }
}