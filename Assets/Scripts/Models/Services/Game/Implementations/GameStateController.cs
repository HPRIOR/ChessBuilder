using System;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Implementations
{
    public class GameStateController : IGameStateController, ITurnEventInvoker
    {
        private readonly IGameStateUpdater _gameStateUpdater;

        public GameStateController(
            IGameStateUpdater gameStateUpdater
        )
        {
            _gameStateUpdater = gameStateUpdater;
            Turn = PieceColour.Black;
        }


        public GameState CurrentGameState { get; private set; }
        public PieceColour Turn { get; private set; }

        public void UpdateGameState(BoardState newState)
        {
            Turn = NextTurn();
            var previousState = CurrentGameState?.BoardState;

            CurrentGameState = _gameStateUpdater.UpdateGameState(newState, Turn);

            GameStateChangeEvent?.Invoke(previousState, CurrentGameState.BoardState);
        }

        /// <summary>
        ///     Emits event with current board state
        /// </summary>
        public void RetainBoardState()
        {
            GameStateChangeEvent?.Invoke(CurrentGameState.BoardState, CurrentGameState.BoardState);
        }

        // TODO pass in GameState rather than board state
        public event Action<BoardState, BoardState> GameStateChangeEvent;

        private PieceColour NextTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
    }
}