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
        private const int maxBuildPoints = 39;
        private readonly IAllPossibleMovesGenerator _allPossibleMovesGenerator;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IBuildPointsCalculator _buildPointsCalculator;

        // TODO Don't hard code max build points 
        public GameStateController(
            IAllPossibleMovesGenerator allPossibleMovesGenerator,
            IBuildMoveGenerator buildMoveGenerator,
            IBuildPointsCalculator buildPointsCalculator
        )
        {
            _allPossibleMovesGenerator = allPossibleMovesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _buildPointsCalculator = buildPointsCalculator;
            BlackState = new PlayerState(maxBuildPoints);
            WhiteState = new PlayerState(maxBuildPoints);
            Turn = PieceColour.White;
        }

        public BoardState CurrentBoardState { get; private set; }
        public PieceColour Turn { get; private set; }
        public PlayerState BlackState { get; private set; }
        public PlayerState WhiteState { get; private set; }
        public IDictionary<Position, HashSet<PieceType>> PossibleBuildMoves { get; private set; }
        public IDictionary<Position, HashSet<Position>> PossiblePieceMoves { get; private set; }

        public void UpdateBoardState(BoardState newState)
        {
            var previousState = CurrentBoardState;
            CurrentBoardState = newState;

            // TODO: this may only need to change for the current turn
            BlackState =
                _buildPointsCalculator.CalculateBuildPoints(PieceColour.Black, CurrentBoardState, maxBuildPoints);
            WhiteState =
                _buildPointsCalculator.CalculateBuildPoints(PieceColour.White, CurrentBoardState, maxBuildPoints);

            PossiblePieceMoves = _allPossibleMovesGenerator.GetPossibleMoves(CurrentBoardState, Turn);

            var relevantPlayerState = Turn == PieceColour.Black ? BlackState : WhiteState;
            PossibleBuildMoves =
                _buildMoveGenerator.GetPossibleBuildMoves(CurrentBoardState, Turn, relevantPlayerState);

            Turn = ChangeTurn();
            GameStateChangeEvent?.Invoke(previousState, CurrentBoardState);
        }

        /// <summary>
        ///     Tells UI to update with previous board state
        /// </summary>
        public void RetainBoardState()
        {
            GameStateChangeEvent?.Invoke(CurrentBoardState, CurrentBoardState);
        }

        public event Action<BoardState, BoardState> GameStateChangeEvent;

        private PieceColour ChangeTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
    }
}