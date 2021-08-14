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
using Zenject;

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

        public GameStateUpdater(GameState gameState, IMovesGenerator movesGenerator,
            IBuildMoveGenerator buildMoveGenerator,
            IBuildPointsCalculator buildPointsCalculator, IBuildResolver buildResolver, IGameOverEval gameOverEval,
            IBuilder builder, IPieceMover mover)
        {
            GameState = gameState;
            _movesGenerator = movesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _buildPointsCalculator = buildPointsCalculator;
            _buildResolver = buildResolver;
            _gameOverEval = gameOverEval;
            _builder = builder;
            _mover = mover;
        }

        public GameState GameState { get; }

        /*
         * GameState will be a member of this class, which will be mutated, instead of generated each turn
         * instead of returning void, this method will return a 'history' of the changes which have occured
         */
        public void UpdateGameState(Position from, Position to, PieceColour turn)
        {
            // GenerateNewBoardState will mutate the board passed in and return change history
            _mover.ModifyBoardState(GameState.BoardState, from, to);
            // don't pass board state, use GameState.BoardState
            UpdateGameState(turn);
        }

        public void UpdateGameState(Position buildPosition, PieceType piece, PieceColour turn)
        {
            _builder.GenerateNewBoardState(GameState.BoardState, buildPosition, piece);
            UpdateGameState(turn);
        }

        public void UpdateGameState(PieceColour turn)
        {
            // opposite turn from current needs to be passed to build resolver 
            // this is due to builds being resolved at the end of a players turn - not the start of their turn
            _buildResolver.ResolveBuilds(GameState.BoardState, NextTurn(turn));

            var (blackState, whiteState) = GetPlayerState(GameState.BoardState);

            // generate new build state but changeMoveState instead of producing new variable
            var moveState = _movesGenerator.GetPossibleMoves(GameState.BoardState, turn);
            GameState.PossiblePieceMoves = moveState.PossibleMoves;
            GameState.Check = moveState.Check;

            var relevantPlayerState = turn == PieceColour.Black ? blackState : whiteState;
            // generate new build state but change possible build moves in GameState instead of producing a new one
            GameState.PossibleBuildMoves =
                GetPossibleBuildMoves(GameState.BoardState, turn, moveState, relevantPlayerState);

            GameState.CheckMate = _gameOverEval.CheckMate(moveState.Check, moveState.PossibleMoves);
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

        public class Factory : PlaceholderFactory<GameState, GameStateUpdater>
        {
        }
    }
}