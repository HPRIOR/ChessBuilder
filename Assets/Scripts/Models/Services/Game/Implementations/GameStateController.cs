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

        public event Action<BoardState, BoardState> GameStateChangeEvent;

        private PieceColour NextTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;

        public override string ToString() => "";
        // var stringBuilder = new StringBuilder();
        // stringBuilder.Append("Black state: \n");
        // stringBuilder.Append($"     Build Points: {BlackState.BuildPoints} \n");
        // stringBuilder.Append("White state: \n");
        // stringBuilder.Append($"     Build Points: {WhiteState.BuildPoints} \n");
        // stringBuilder.Append("Possible Moves: \n");
        // PossiblePieceMoves.Keys.ToList().ForEach(piecePosition =>
        // {
        //     stringBuilder.Append(
        //         $"     {CurrentBoardState.Board[piecePosition.X, piecePosition.Y].CurrentPiece.Type}: \n       ");
        //     PossiblePieceMoves[piecePosition].ToList().ForEach(move => stringBuilder.Append($"({move}), "));
        // });
        // stringBuilder.Append("\n");
        // stringBuilder.Append("Possible Build Pieces: \n     ");
        // PossibleBuildMoves.BuildPieces.ToList().ForEach(piece => stringBuilder.Append($"{piece}, "));
        // stringBuilder.Append("\n");
        // stringBuilder.Append("Possible Build Positions: \n     ");
        // PossibleBuildMoves.BuildPositions.OrderBy(p => p.Y).ToList()
        //     .ForEach(position => stringBuilder.Append($"({position}), "));
        // return stringBuilder.ToString();
    }
}