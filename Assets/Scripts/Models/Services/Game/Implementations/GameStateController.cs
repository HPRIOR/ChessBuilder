using System;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Implementations
{
    public class GameStateController : IGameStateController, ITurnEventInvoker
    {
        private readonly GameInitializer _gameInitializer;
        private readonly IGameStateUpdater _gameStateUpdater;

        public GameStateController(
            IGameStateUpdater gameStateUpdater, GameInitializer gameInitializer
        )
        {
            _gameStateUpdater = gameStateUpdater;
            _gameInitializer = gameInitializer;
            Turn = PieceColour.White;
        }


        public GameState CurrentGameState { get; private set; }
        public PieceColour Turn { get; private set; }


        public void UpdateGameState(GameState newGameState)
        {
            var previousState = CurrentGameState?.BoardState.Clone();
            CurrentGameState = newGameState;
            GameStateChangeEvent?.Invoke(previousState, CurrentGameState.BoardState);
            Turn = NextTurn();
        }

        public void InitializeGame(BoardState boardState)
        {
            CurrentGameState = _gameInitializer.InitialiseGame(boardState);
            RetainBoardState();
        }

        public void UpdateGameState(BoardState newBoardState)
        {
            Turn = NextTurn();
            var previousState = CurrentGameState?.BoardState.Clone();

            // When white/black turn has been executed, game state needs to be set up in this method for the opposite player
            // hence why NextTurn() is called at the top of the method
            CurrentGameState = _gameStateUpdater.UpdateGameState(newBoardState, Turn);

            GameStateChangeEvent?.Invoke(previousState, CurrentGameState.BoardState);
        }

        // public void UpdateGameState(Position from, Position to)

        // public void UpdateGameState(Position at, PieceType piece)

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