using System.Collections.Immutable;
using Models.Services.Build.Interfaces;
using Models.Services.Game.Interfaces;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.MoveState;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Game.Implementations
{
    public class GameStateUpdater : IGameStateUpdater
    {
        //TODO inject me
        private const int maxBuildPoints = 39;
        private readonly IBuilder _builder;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IBuildPointsCalculator _buildPointsCalculator;
        private readonly IBuildResolver _buildResolver;
        private readonly IGameOverEval _gameOverEval;
        private readonly IPieceMover _mover;
        private readonly IMovesGenerator _movesGenerator;

        public GameStateUpdater(IMovesGenerator movesGenerator, IBuildMoveGenerator buildMoveGenerator,
            IBuildPointsCalculator buildPointsCalculator, IBuildResolver buildResolver, IGameOverEval gameOverEval,
            IBuilder builder, IPieceMover mover)
        {
            _movesGenerator = movesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _buildPointsCalculator = buildPointsCalculator;
            _buildResolver = buildResolver;
            _gameOverEval = gameOverEval;
            _builder = builder;
            _mover = mover;
        }

        public GameState UpdateGameState(GameState previousGameState, Position from, Position to, PieceColour turn)
        {
            var newBoardState = _mover.GenerateNewBoardState(previousGameState.BoardState, from, to);
            return UpdateGameState(newBoardState, turn);
        }

        public GameState UpdateGameState(GameState previousGameState, Position buildPosition, PieceType piece,
            PieceColour turn)
        {
            var newBoardState = _builder.GenerateNewBoardState(previousGameState.BoardState, buildPosition, piece);
            return UpdateGameState(newBoardState, turn);
        }

        public GameState UpdateGameState(BoardState newBoardState, PieceColour turn)
        {
            // opposite turn from current needs to be passed to build resolver 
            // this is due to builds being resolved at the end of a players turn - not the start of their turn
            _buildResolver.ResolveBuilds(newBoardState, NextTurn(turn));

            var (blackState, whiteState) = GetPlayerState(newBoardState);

            var moveState = _movesGenerator.GetPossibleMoves(newBoardState, turn);

            var relevantPlayerState = turn == PieceColour.Black ? blackState : whiteState;
            var possibleBuildMoves = GetPossibleBuildMoves(newBoardState, turn, moveState, relevantPlayerState);

            var checkMate = _gameOverEval.CheckMate(moveState.Check, moveState.PossibleMoves);

            return new GameState(moveState.Check, checkMate, blackState, whiteState, moveState.PossibleMoves,
                possibleBuildMoves, newBoardState);
        }

        private BuildMoves GetPossibleBuildMoves(BoardState newBoardState, PieceColour turn, MoveState moveState,
            PlayerState relevantPlayerState) =>
            moveState.Check
                ? new BuildMoves(ImmutableHashSet<Position>.Empty,
                    ImmutableHashSet<PieceType>.Empty) // no build moves when in check
                : _buildMoveGenerator.GetPossibleBuildMoves(newBoardState, turn, relevantPlayerState);

        //TODO: calculate only the relevant player state
        private (PlayerState blackState, PlayerState whiteState) GetPlayerState(BoardState newBoardState)
        {
            var blackState =
                _buildPointsCalculator.CalculateBuildPoints(PieceColour.Black, newBoardState, maxBuildPoints);
            var whiteState =
                _buildPointsCalculator.CalculateBuildPoints(PieceColour.White, newBoardState, maxBuildPoints);
            return (blackState, whiteState);
        }

        private static PieceColour NextTurn(PieceColour turn) =>
            turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
    }
}