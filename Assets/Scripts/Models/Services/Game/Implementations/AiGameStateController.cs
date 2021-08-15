using System;
using Models.Services.AI.Implementations;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Implementations
{
    public class AiGameStateController : IGameStateController, ITurnEventInvoker
    {
        private readonly AiMoveGenerator _aiMoveGenerator;
        private readonly GameInitializer _gameInitializer;
        private readonly GameStateUpdaterFactory _gameStateUpdaterFactory;
        private IGameStateUpdater _gameStateUpdater;

        public AiGameStateController(
            GameStateUpdaterFactory gameStateUpdaterFactory, GameInitializer gameInitializer,
            AiMoveGenerator aiMoveGenerator
        )
        {
            _gameStateUpdaterFactory = gameStateUpdaterFactory;
            _gameInitializer = gameInitializer;
            _aiMoveGenerator = aiMoveGenerator;
            Turn = PieceColour.White;
        }


        public GameState CurrentGameState { get; private set; }
        public PieceColour Turn { get; private set; }


        public void UpdateGameState(Position from, Position to)
        {
            throw new NotImplementedException();
        }

        public void InitializeGame(BoardState boardState)
        {
            CurrentGameState = _gameInitializer.InitialiseGame(boardState);
            _gameStateUpdater = _gameStateUpdaterFactory.Create(CurrentGameState);
            RetainBoardState();
        }

        public void UpdateGameState(BoardState newBoardState)
        {
            Turn = NextTurn();
            var previousState = CurrentGameState?.BoardState.Clone();

            // Game state updater sets up the possible moves for the next player 
            // hence why NextTurn() is called at the top of the method
            _gameStateUpdater.UpdateGameState(Turn);
            CurrentGameState = _gameStateUpdater.GameState;
            GameStateChangeEvent?.Invoke(previousState, CurrentGameState.BoardState);
            // Invoke some other mechanism to call overloaded Update game state

            var move = _aiMoveGenerator.GetMove(CurrentGameState, 3, Turn);

            previousState = CurrentGameState.BoardState;
            CurrentGameState = move(CurrentGameState, Turn);

            GameStateChangeEvent?.Invoke(previousState, CurrentGameState.BoardState);
            Turn = NextTurn();
        }

        public void RevertGameState()
        {
            throw new NotImplementedException();
        }

        public void UpdateGameState(Position buildPiece, PieceType piece)
        {
            throw new NotImplementedException();
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

        public void UpdateGameState(GameState newGameState)
        {
            var previousState = CurrentGameState?.BoardState.Clone();
            CurrentGameState = newGameState;
            GameStateChangeEvent?.Invoke(previousState, CurrentGameState.BoardState);
            Turn = NextTurn();
        }

        private PieceColour NextTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
    }
}