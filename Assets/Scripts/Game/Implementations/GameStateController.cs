using System;
using System.Collections.Generic;
using Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Interfaces;
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

        public IBoardState CurrentBoardState { get; private set; }

        public IDictionary<IBoardPosition, HashSet<IBoardPosition>> PossiblePieceMoves { get; private set; }

        public PieceColour Turn { get; private set; } = PieceColour.White;

        // provide an overload which passes in the changed tile. This can be used to check for mate when passed
        // to generate possible piece moves
        public void UpdateGameState(IBoardState newState)
        {
            var previousState = CurrentBoardState;
            CurrentBoardState = newState;

            PossiblePieceMoves = _allPossibleMovesGenerator.GetPossibleMoves(CurrentBoardState, Turn);

            Turn = ChangeTurn();
            GameStateChangeEvent?.Invoke(previousState, CurrentBoardState);
        }

        public event Action<IBoardState, IBoardState> GameStateChangeEvent;

        private PieceColour ChangeTurn()
        {
            return Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
        }
    }
}