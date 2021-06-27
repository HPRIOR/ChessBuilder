using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Interfaces;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.State.PlayerState;
using UnityEngine;

namespace Game.Implementations
{
    public class GameStateController : IGameState, ITurnEventInvoker
    {
        private const int maxBuildPoints = 39;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IBuildPointsCalculator _buildPointsCalculator;
        private readonly IMovesGenerator _movesGenerator;

        // TODO Don't hard code max build points 
        public GameStateController(
            IMovesGenerator movesGenerator,
            IBuildMoveGenerator buildMoveGenerator,
            IBuildPointsCalculator buildPointsCalculator
        )
        {
            _movesGenerator = movesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _buildPointsCalculator = buildPointsCalculator;
            BlackState = new PlayerState(maxBuildPoints);
            WhiteState = new PlayerState(maxBuildPoints);
            Turn = PieceColour.White;
        }

        public bool Check { get; private set; }
        public BoardState CurrentBoardState { get; private set; }
        public PieceColour Turn { get; private set; }
        public PlayerState BlackState { get; private set; }
        public PlayerState WhiteState { get; private set; }
        public BuildMoves PossibleBuildMoves { get; private set; }
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

            var movesState = _movesGenerator.GetPossibleMoves(CurrentBoardState, Turn);
            PossiblePieceMoves = movesState.PossibleMoves;
            Check = movesState.Check;

            var relevantPlayerState = Turn == PieceColour.Black ? BlackState : WhiteState;
            PossibleBuildMoves =
                Check
                    ? new BuildMoves(new HashSet<Position>(), new HashSet<PieceType>()) // no build move when in check
                    : _buildMoveGenerator.GetPossibleBuildMoves(CurrentBoardState, Turn, relevantPlayerState);

            Turn = ChangeTurn();
            GameStateChangeEvent?.Invoke(previousState, CurrentBoardState);
            Debug.Log(this);
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

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Black state: \n");
            stringBuilder.Append($"     Build Points: {BlackState.BuildPoints} \n");
            stringBuilder.Append("White state: \n");
            stringBuilder.Append($"     Build Points: {WhiteState.BuildPoints} \n");
            stringBuilder.Append("Possible Moves: \n");
            PossiblePieceMoves.Keys.ToList().ForEach(piecePosition =>
            {
                stringBuilder.Append(
                    $"     {CurrentBoardState.Board[piecePosition.X, piecePosition.Y].CurrentPiece.Type}: \n       ");
                PossiblePieceMoves[piecePosition].ToList().ForEach(move => stringBuilder.Append($"({move}), "));
            });
            stringBuilder.Append("\n");
            stringBuilder.Append("Possible Build Pieces: \n     ");
            PossibleBuildMoves.BuildPieces.ToList().ForEach(piece => stringBuilder.Append($"{piece}, "));
            stringBuilder.Append("\n");
            stringBuilder.Append("Possible Build Positions: \n     ");
            PossibleBuildMoves.BuildPositions.OrderBy(p => p.Y).ToList()
                .ForEach(position => stringBuilder.Append($"({position}), "));
            return stringBuilder.ToString();
        }
    }
}