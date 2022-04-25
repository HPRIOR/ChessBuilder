using System;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.Game.Implementations
{
    public sealed class GameStateController : IGameStateController, ITurnEventInvoker
    {
        private readonly GameInitializer _gameInitializer;
        private readonly IGameStateUpdater _gameStateUpdater;


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
            CurrentGameState = _gameInitializer.InitialiseGame(boardState);
            _gameStateUpdater.SetInitialGameState(CurrentGameState);
            RetainBoardState();
        }

        public void RevertGameState()
        {
            CurrentGameState = _gameStateUpdater.RevertGameState();
            Turn = NextTurn();
        }


        public void UpdateGameState(Position from, Position to)
        {
            Turn = NextTurn();
            var previousBoardState = CurrentGameState?.BoardState.Clone();

            CurrentGameState = _gameStateUpdater.UpdateGameState(from, to, Turn);
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

        public bool IsValidMove(Position buildPosition, PieceType piece)
        {
            var buildMoves = CurrentGameState.PossibleBuildMoves;
            return buildMoves.BuildPositions.Contains(buildPosition) && buildMoves.BuildPieces.Contains(piece);
        }

        public bool IsValidMove(Position from, Position to)
        {
            if (from == to) return false;
            var possibleMoves = CurrentGameState.PossiblePieceMoves;
            if (possibleMoves.ContainsKey(from))
                return possibleMoves[from].Contains(to);
            return false;
        }

        // TODO pass in GameState rather than board state
        public event Action<BoardState, BoardState> GameStateChangeEvent;

        private PieceColour NextTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
    }
}