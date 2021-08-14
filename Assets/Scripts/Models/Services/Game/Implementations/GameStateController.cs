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
        private readonly GameStateUpdaterFactory _gameStateUpdaterFactory;
        private IGameStateUpdater _gameStateUpdater;


        public GameStateController(
            GameStateUpdaterFactory gameStateUpdaterFactory, GameInitializer gameInitializer
        )
        {
            _gameInitializer = gameInitializer;
            _gameStateUpdaterFactory = gameStateUpdaterFactory;
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

        /// <summary>
        ///     this is redundant for now. It was used to revert state back by passing in an old board state, however the
        ///     game state updater no longer supports this functionality. Some new way of reverting changes is needed.
        ///     Should remove and fix tests and anything else which relies on this.
        /// </summary>
        /// <param name="newBoardState"></param>
        public void UpdateGameState(BoardState newBoardState)
        {
            Turn = NextTurn();
            var previousState = CurrentGameState?.BoardState.Clone();

            // When white/black turn has been executed, game state needs to be set up in this method for the opposite player
            // hence why NextTurn() is called at the top of the method
            _gameStateUpdater.UpdateGameState(Turn);
            CurrentGameState = _gameStateUpdater.GameState;

            GameStateChangeEvent?.Invoke(previousState, CurrentGameState.BoardState);
        }

        public void UpdateGameState(Position from, Position to)
        {
            Turn = NextTurn();
            var previousBoardState = CurrentGameState?.BoardState.Clone();
            _gameStateUpdater.UpdateGameState(from, to, Turn);
            CurrentGameState = _gameStateUpdater.GameState;

            GameStateChangeEvent?.Invoke(previousBoardState, CurrentGameState.BoardState);
        }

        public void UpdateGameState(Position buildPosition, PieceType piece)
        {
            Turn = NextTurn();
            var previousBoardState = CurrentGameState?.BoardState.Clone();
            _gameStateUpdater.UpdateGameState(buildPosition, piece, Turn);
            CurrentGameState = _gameStateUpdater.GameState;

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