using System;
using System.Collections.Generic;
using Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;

namespace Game.Implementations
{
    public class GameStateController : IGameState, ITurnEventInvoker
    {
        private readonly IAllPossibleMovesGenerator _allPossibleMovesGenerator;

        public GameStateController(IAllPossibleMovesGenerator allPossibleMovesGenerator)
        {
            _allPossibleMovesGenerator = allPossibleMovesGenerator;
        }

        public BoardState CurrentBoardState { get; private set; }

        public IDictionary<BoardPosition, HashSet<BoardPosition>> PossiblePieceMoves { get; private set; }

        public PieceColour Turn { get; private set; } = PieceColour.White;

        // provide an overload which passes in the changed tile. This can be used to check for mate when passed
        // to generate possible piece moves
        public void UpdateBoardState(BoardState newState)
        {
            var previousState = CurrentBoardState;
            CurrentBoardState = newState;

            PossiblePieceMoves = _allPossibleMovesGenerator.GetPossibleMoves(CurrentBoardState, Turn);

            Turn = ChangeTurn();
            GameStateChangeEvent?.Invoke(previousState, CurrentBoardState);
        }

        public void RetainBoardState()
        {
            GameStateChangeEvent?.Invoke(CurrentBoardState, CurrentBoardState);
        }

        public event Action<BoardState, BoardState> GameStateChangeEvent;

        private PieceColour ChangeTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
    }
}