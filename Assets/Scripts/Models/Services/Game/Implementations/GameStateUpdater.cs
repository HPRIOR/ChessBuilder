using System.Collections.Generic;
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
    public sealed class GameStateUpdater : IGameStateUpdater
    {
        // TODO: inject me 
        private const int MaxBuildPoints = 39;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IBuildPointsCalculator _buildPointsCalculator;
        private readonly IBuildResolver _buildResolver;
        private readonly IGameOverEval _gameOverEval;
        private readonly IMovesGenerator _movesGenerator;

        public GameStateUpdater(
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
        }

        private readonly Stack<GameState> _gameStateHistory = new Stack<GameState>();

        public GameState UpdateGameState(Position from, Position to, PieceColour turn)
        {
            var previousGameState = _gameStateHistory.Peek();
            var newBoardState = GenerateNewBoardStateWithMove(previousGameState.BoardState, from, to);
            return UpdateGameState(turn, newBoardState);
        }

        public GameState UpdateGameState(Position buildPosition, PieceType piece, PieceColour turn)
        {
            var previousGameState = _gameStateHistory.Peek();
            var newBoardState = GenerateNewBoardStateWithBuild(previousGameState.BoardState, buildPosition, piece);
            return UpdateGameState(turn, newBoardState);
        }


        private GameState UpdateGameState(PieceColour turn, BoardState boardState)
        {
            // opposite turn from current needs to be passed to build resolver 
            // this is due to builds being resolved at the end of a players turn - not the start of their turn
            _buildResolver.ResolveBuilds(boardState, NextTurn(turn));

            var playerState = GetPlayerState(boardState, turn);
            var moveState = _movesGenerator.GetPossibleMoves(boardState, turn);

            var possiblePieceMoves = moveState.PossibleMoves;
            var possibleBuildMoves = GetPossibleBuildMoves(boardState, turn, moveState, playerState);

            var check = moveState.Check;
            var checkMate = _gameOverEval.CheckMate(moveState.Check, moveState.PossibleMoves);

            var newGameState =
                new GameState(
                    check,
                    checkMate,
                    playerState,
                    possiblePieceMoves,
                    possibleBuildMoves,
                    boardState);

            _gameStateHistory.Push(newGameState);
            return newGameState;
        }


        private BuildMoves GetPossibleBuildMoves(BoardState newBoardState, PieceColour turn, MoveState moveState,
            PlayerState relevantPlayerState) =>
            moveState.Check
                ? new BuildMoves(new List<Position>(), new List<PieceType>()) // no build moves when in check
                : _buildMoveGenerator.GetPossibleBuildMoves(newBoardState, turn, relevantPlayerState);

        private PlayerState GetPlayerState(BoardState newBoardState, PieceColour turn)
            => _buildPointsCalculator.CalculateBuildPoints(turn, newBoardState, MaxBuildPoints);

        private static PieceColour NextTurn(PieceColour turn) =>
            turn == PieceColour.White ? PieceColour.Black : PieceColour.White;

        private BoardState GenerateNewBoardStateWithBuild(BoardState boardState, Position buildPosition,
            PieceType piece)
        {
            // add build positions to active pieces 
            boardState.ActiveBuilds.Add(buildPosition);

            // modify board state 
            boardState.Board[buildPosition.X][buildPosition.Y].BuildTileState = new BuildTileState(piece);
        }

        private BoardState GenerateNewBoardStateWithMove(BoardState boardState, Position from,
            Position destination)
        {
            ref var destinationTile = ref boardState.GetTileAt(destination);
            ref var fromTile = ref boardState.GetTileAt(from);

            var isTakingMove = destinationTile.CurrentPiece != PieceType.NullPiece;

            boardState.ActivePieces.Remove(from);
            if (!isTakingMove)
                boardState.ActivePieces.Add(destination);

            // swap pieces
            destinationTile.CurrentPiece = fromTile.CurrentPiece;
            fromTile.CurrentPiece = PieceType.NullPiece;

            // return nothing 
        }
    }
}