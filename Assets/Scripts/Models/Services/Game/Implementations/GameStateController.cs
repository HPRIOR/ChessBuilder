using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common;
using Mirror;
using Models.Services.Build.Interfaces;
using Models.Services.Game.Interfaces;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.MoveState;
using Models.State.PieceState;
using Models.State.PlayerState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Game.Implementations
{
    public sealed class GameStateController : IGameStateController, ITurnEventInvoker
    {
        private const int MaxBuildPoints = 39;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly GameInitializer _gameInitializer;
        private readonly IGameOverEval _gameOverEval;
        private readonly Stack<GameState> _gameStateHistory = new();
        private readonly IMovesGenerator _movesGenerator;

        public GameStateController(
            IBuildMoveGenerator buildMoveGenerator,
            IGameOverEval gameOverEval,
            IMovesGenerator movesGenerator,
            GameInitializer gameInitializer
        )
        {
            _buildMoveGenerator = buildMoveGenerator;
            _gameOverEval = gameOverEval;
            _movesGenerator = movesGenerator;
            _gameInitializer = gameInitializer;
            Turn = PieceColour.White;
        }

        public GameState CurrentGameState { get; private set; }
        public PieceColour Turn { get; private set; }

        public void InitializeGame(BoardState boardState)
        {
            CurrentGameState = _gameInitializer.InitialiseGame(boardState);
            _gameStateHistory.Push(CurrentGameState);
            RetainBoardState();
        }

        public void RevertGameState()
        {
            if (_gameStateHistory.Count > 1)
            {
                var previousGameState = CurrentGameState;
                var previousBoardState = CurrentGameState.BoardState;
                CurrentGameState = _gameStateHistory.Pop();
                Turn = NextTurn();
                BoardStateChangeEvent?.Invoke(previousBoardState, CurrentGameState.BoardState);
                GameStateChangeEvent?.Invoke(previousGameState, CurrentGameState);
            }
        }


        public void UpdateGameState(Position from, Position to)
        {
            Turn = NextTurn();
            var previousGameState = CurrentGameState;
            var previousBoardState = CurrentGameState?.BoardState.Clone();

            CurrentGameState = UpdateGameState(from, to, Turn);
            BoardStateChangeEvent?.Invoke(previousBoardState, CurrentGameState.BoardState);
            GameStateChangeEvent?.Invoke(previousGameState, CurrentGameState);
        }

        public void UpdateGameState(Position buildPosition, PieceType piece)
        {
            Turn = NextTurn();
            var previousGameState = CurrentGameState;
            var previousBoardState = CurrentGameState?.BoardState.Clone();

            CurrentGameState = UpdateGameState(buildPosition, piece, Turn);
            BoardStateChangeEvent?.Invoke(previousBoardState, CurrentGameState.BoardState);
            GameStateChangeEvent?.Invoke(previousGameState, CurrentGameState);
        }

        /// <summary>
        ///     Emits event with current board state
        /// </summary>
        public void RetainBoardState()
        {
            BoardStateChangeEvent?.Invoke(CurrentGameState.BoardState, CurrentGameState.BoardState);
            GameStateChangeEvent?.Invoke(CurrentGameState, CurrentGameState);
        }

        public bool IsValidMove(Position buildPosition, PieceType piece)
        {
            var buildMoves = CurrentGameState.PossibleBuildMoves;
            return buildMoves.BuildPositions.Contains(buildPosition) && buildMoves.BuildPieces.Contains(piece);
        }

        public bool IsValidMove(Position from, Position to)
        {
            if (from == to) return false;
            var possibleMoves = CurrentGameState.PossiblePieceMoves;
            if (possibleMoves.ContainsKey(from))
                return possibleMoves[from].Contains(to);
            return false;
        }

        public event Action<BoardState, BoardState> BoardStateChangeEvent;
        public event Action<GameState, GameState> GameStateChangeEvent;


        private GameState UpdateGameState(Position from, Position to, PieceColour turn)
        {
            var previousGameState = _gameStateHistory.Peek();
            var newBoardState = previousGameState.BoardState.WithMove(from, to, turn);
            return UpdateGameState(turn, newBoardState);
        }

        private GameState UpdateGameState(Position buildPosition, PieceType piece, PieceColour turn)
        {
            var previousGameState = _gameStateHistory.Peek();
            var newBoardState = previousGameState.BoardState.WithBuild(buildPosition, piece, turn);
            return UpdateGameState(turn, newBoardState);
        }

        private GameState UpdateGameState(PieceColour turn, BoardState boardState)
        {
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
            => CalculateBuildPoints(turn, newBoardState, MaxBuildPoints);

        private PlayerState CalculateBuildPoints(PieceColour pieceColour, BoardState boardState,
            int maxPoints)
        {
            var result = 0;
            var activeBuilds = boardState.ActiveBuilds;
            var activePieces = boardState.ActivePieces;
            var activePositions = activeBuilds.Union(activePieces);

            foreach (var pos in activePositions)
            {
                var tile = boardState.GetTileAt(pos);
                var pieceIsOfColourType = tile.CurrentPiece.Colour() == pieceColour &&
                                          tile.CurrentPiece != PieceType.NullPiece;
                if (pieceIsOfColourType)
                    result += tile.CurrentPiece.Value();

                var pieceOfColourIsBeingBuilt = tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                                                tile.BuildTileState.BuildingPiece.Colour() == pieceColour;
                if (pieceOfColourIsBeingBuilt)
                    result += tile.BuildTileState.BuildingPiece.Value();
            }

            return new PlayerState(maxPoints - result);
        }

        private PieceColour NextTurn() => Turn == PieceColour.White ? PieceColour.Black : PieceColour.White;
    }
}