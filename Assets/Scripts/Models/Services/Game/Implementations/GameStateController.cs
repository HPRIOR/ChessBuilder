using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Models.Services.Build.Interfaces;
using Models.Services.Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Game.Implementations
{
    public class GameStateController : IGameState, ITurnEventInvoker
    {
        private const int maxBuildPoints = 39;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IBuildPointsCalculator _buildPointsCalculator;
        private readonly IBuildResolver _buildResolver;
        private readonly IGameOverEval _gameOverEval;
        private readonly IMovesGenerator _movesGenerator;

        // TODO Don't hard code max build points 
        public GameStateController(
            IMovesGenerator movesGenerator,
            IBuildMoveGenerator buildMoveGenerator,
            IBuildPointsCalculator buildPointsCalculator,
            IBuildResolver buildResolver,
            IGameOverEval gameOverEval
        )
        {
            _movesGenerator = movesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _buildPointsCalculator = buildPointsCalculator;
            _buildResolver = buildResolver;
            _gameOverEval = gameOverEval;
            BlackState = new PlayerState(maxBuildPoints);
            WhiteState = new PlayerState(maxBuildPoints);
            Turn = PieceColour.Black;
        }

        public bool Check { get; private set; }
        public bool CheckMate { get; private set; }
        public BoardState CurrentBoardState { get; private set; }
        public PieceColour Turn { get; private set; }
        public PlayerState BlackState { get; private set; }
        public PlayerState WhiteState { get; private set; }
        public BuildMoves PossibleBuildMoves { get; private set; }
        public ImmutableDictionary<Position, ImmutableHashSet<Position>> PossiblePieceMoves { get; private set; }

        public void UpdateBoardState(BoardState newState)
        {
            Turn = NextTurn();
            var previousState = CurrentBoardState;
            CurrentBoardState = newState;

            // opposite turn from current needs to be passed to build resolver 
            // this is due to builds being resolved at the end of a players turn - not the start of their turn
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
                    ? new BuildMoves(ImmutableHashSet<Position>.Empty,
                        ImmutableHashSet<PieceType>.Empty) // no build moves when in check
                    : _buildMoveGenerator.GetPossibleBuildMoves(CurrentBoardState, Turn, relevantPlayerState);

            CheckMate = _gameOverEval.CheckMate(Check, PossiblePieceMoves);

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