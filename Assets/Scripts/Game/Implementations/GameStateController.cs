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

namespace Game.Implementations
{
    public class GameStateController : IGameState, ITurnEventInvoker
    {
        private const int maxBuildPoints = 39;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IBuildPointsCalculator _buildPointsCalculator;
        private readonly IBuildResolver _buildResolver;
        private readonly IMovesGenerator _movesGenerator;

        // TODO Don't hard code max build points 
        public GameStateController(
            IMovesGenerator movesGenerator,
            IBuildMoveGenerator buildMoveGenerator,
            IBuildPointsCalculator buildPointsCalculator,
            IBuildResolver buildResolver
        )
        {
            _movesGenerator = movesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _buildPointsCalculator = buildPointsCalculator;
            _buildResolver = buildResolver;
            BlackState = new PlayerState(maxBuildPoints);
            WhiteState = new PlayerState(maxBuildPoints);
            Turn = PieceColour.Black;
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
            Turn = NextTurn();
            var previousState = CurrentBoardState;
            CurrentBoardState = newState;

            _buildResolver.ResolveBuilds(CurrentBoardState, NextTurn());

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
                    ? new BuildMoves(new HashSet<Position>(), new HashSet<PieceType>()) // no build moves when in check
                    : _buildMoveGenerator.GetPossibleBuildMoves(CurrentBoardState, Turn, relevantPlayerState);

            GameStateChangeEvent?.Invoke(previousState, CurrentBoardState);
        }

        /// <summary>
        ///     Emits event with current board state
        /// </summary>
        public void RetainBoardState()
        {
            GameStateChangeEvent?.Invoke(CurrentBoardState, CurrentBoardState);
        }

        public event Action<BoardState, BoardState> GameStateChangeEvent;

        private PieceColour NextTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;

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