using System;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Implementations
{
    /*
     * GamestateUpdater should be return new game state, and game state should become immutable.
     * Keeping track of the current and historical game states will be the responsibility of this class
     */
    public sealed class GameStateController : IGameStateController, ITurnEventInvoker
    {
        private readonly GameInitializer _gameInitializer;
        private readonly GameStateUpdaterFactory _gameStateUpdaterFactory;
        private IGameStateUpdater _gameStateUpdater;
        

        public GameStateController(
            IGameStateUpdater gameStateUpdater, GameInitializer gameInitializer
        )
        {
            _gameInitializer = gameInitializer;
            _gameStateUpdater = gameStateUpdater;
            Turn = PieceColour.White;
        }

        public GameState CurrentGameState { get; private set; }
        public PieceColour Turn { get; private set; }

        public void InitializeGame(BoardState boardState)
        {
            // pass this GameState to GameStateUpdater
            CurrentGameState = _gameInitializer.InitialiseGame(boardState);
            _gameStateUpdater = _gameStateUpdaterFactory.Create(CurrentGameState);
            RetainBoardState();
        }

        public void RevertGameState()
        {
            Turn = NextTurn();
        }


        public void UpdateGameState(Position from, Position to)
        {
            Turn = NextTurn();
            var previousBoardState = CurrentGameState?.BoardState.Clone();
            
            CurrentGameState = _gameStateUpdater.UpdateGameState(@from, to, Turn);
            GameStateChangeEvent?.Invoke(previousBoardState, CurrentGameState.BoardState);
        }

        public void UpdateGameState(Position buildPosition, PieceType piece)
        {
            Turn = NextTurn();
            var previousBoardState = CurrentGameState?.BoardState.Clone();
            
            CurrentGameState = _gameStateUpdater.UpdateGameState(buildPosition, piece, Turn);
            GameStateChangeEvent?.Invoke(previousBoardState, CurrentGameState.BoardState);
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