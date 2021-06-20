using System;
using System.Collections.Generic;
using Game.Interfaces;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Game.Implementations
{
    public class GameStateController : IGameState, ITurnEventInvoker
    {
        private readonly IAllPossibleMovesGenerator _allPossibleMovesGenerator;
        private readonly IBuildMoveGenerator _buildMoveGenerator;

        public GameStateController(
            IAllPossibleMovesGenerator allPossibleMovesGenerator,
            IBuildMoveGenerator buildMoveGenerator)
        {
            _allPossibleMovesGenerator = allPossibleMovesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
        }

        public PlayerState BlackState { get; } = new PlayerState(39);
        public PlayerState WhiteState { get; } = new PlayerState(39);
        public IEnumerable<Position> PossibleBuildMoves { get; private set; }

        public BoardState CurrentBoardState { get; private set; }

        public IDictionary<Position, HashSet<Position>> PossiblePieceMoves { get; private set; }

        public PieceColour Turn { get; private set; } = PieceColour.White;

        public void UpdateBoardState(BoardState newState)
        {
            var previousState = CurrentBoardState;
            CurrentBoardState = newState;

            PossiblePieceMoves = _allPossibleMovesGenerator.GetPossibleMoves(CurrentBoardState, Turn);
            PossibleBuildMoves = _buildMoveGenerator.GetPossibleBuildMoves(CurrentBoardState, Turn);

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