using System.Collections.Immutable;
using Models.Services.Board;
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
        private GameStateChanges _gameStateChanges = new GameStateChanges();

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
        public GameStateChanges UpdateGameState(Position from, Position to, PieceColour turn)
        {
            _gameStateChanges = new GameStateChanges(GameState)
            {
                Move = new Move(to, from),
                DecrementedTiles = BuildStateDecrementor.DecrementBuilds(GameState.BoardState)
            };
            _mover.ModifyBoardState(GameState.BoardState, from, to);
            UpdateGameState(turn);
            return _gameStateChanges;
        }

        public GameStateChanges UpdateGameState(Position buildPosition, PieceType piece, PieceColour turn)
        {
            _gameStateChanges = new GameStateChanges(GameState)
            {
                Build = new State.GameState.Build(buildPosition, piece),
                DecrementedTiles = BuildStateDecrementor.DecrementBuilds(GameState.BoardState)
            };
            _builder.GenerateNewBoardState(GameState.BoardState, buildPosition, piece);
            UpdateGameState(turn);
            return _gameStateChanges;
        }

        public void RevertGameStateChanges(GameStateChanges gameStateChanges)
        {
            // revert resolved pieces
            foreach ((var position, var type) in gameStateChanges.ResolvedBuilds)
            {
                GameState.BoardState.Board[position.X, position.Y].CurrentPiece = new Piece(PieceType.NullPiece);
                GameState.BoardState.Board[position.X, position.Y].BuildTileState = new BuildTileState(0, type);
            }

            // revert build moves
            var build = gameStateChanges.Build;
            if (build != null) GameState.BoardState.Board[build.At.X, build.At.Y].BuildTileState = new BuildTileState();

            // increment decremented builds
            foreach (var decrementedTile in gameStateChanges.DecrementedTiles)
            {
                var buildTileState = GameState.BoardState.Board[decrementedTile.X, decrementedTile.Y].BuildTileState;
                if (buildTileState.BuildingPiece != PieceType.NullPiece)
                    GameState.BoardState.Board[decrementedTile.X, decrementedTile.Y].BuildTileState =
                        new BuildTileState(buildTileState.Turns + 1, buildTileState.BuildingPiece);
            }

            // revert moved pieces
            if (gameStateChanges.Move != null)
            {
                var movedToPosition = gameStateChanges.Move.To;
                var movedFromPosition = gameStateChanges.Move.From;
                var movedPiece = GameState.BoardState.Board[movedToPosition.X, movedToPosition.Y].CurrentPiece.Type;

                GameState.BoardState.Board[movedToPosition.X, movedToPosition.Y].CurrentPiece =
                    new Piece(PieceType.NullPiece);

                GameState.BoardState.Board[movedFromPosition.X, movedFromPosition.Y].CurrentPiece =
                    new Piece(movedPiece);
            }

            // restore previous state
            GameState.PossiblePieceMoves = gameStateChanges.PossiblePieceMoves;
            GameState.PossibleBuildMoves = gameStateChanges.BuildMoves;
            GameState.Check = gameStateChanges.Check;
            GameState.WhiteState = gameStateChanges.WhitePlayerState;
            GameState.BlackState = gameStateChanges.BlackPlayerState;
        }

        public void UpdateGameState(PieceColour turn)
        {
            // opposite turn from current needs to be passed to build resolver 
            // this is due to builds being resolved at the end of a players turn - not the start of their turn
            _gameStateChanges.ResolvedBuilds = _buildResolver.ResolveBuilds(GameState.BoardState, NextTurn(turn));

            var (blackState, whiteState) = GetPlayerState(GameState.BoardState);
            GameState.BlackState = blackState;
            GameState.WhiteState = whiteState;

            var moveState = _movesGenerator.GetPossibleMoves(GameState.BoardState, turn);
            GameState.PossiblePieceMoves = moveState.PossibleMoves;

            GameState.Check = moveState.Check;

            var relevantPlayerState = turn == PieceColour.Black ? blackState : whiteState;
            var possibleBuildMoves =
                GetPossibleBuildMoves(GameState.BoardState, turn, moveState, relevantPlayerState);
            GameState.PossibleBuildMoves = possibleBuildMoves;

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