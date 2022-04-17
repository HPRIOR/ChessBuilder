using System.Collections.Generic;
using System.Linq;
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
    public sealed class GameStateUpdater : IGameStateUpdater
    {
        // TODO: inject me 
        private const int MaxBuildPoints = 39;
        private readonly IBuildMoveGenerator _buildMoveGenerator;
        private readonly IGameOverEval _gameOverEval;

        private readonly Stack<GameState> _gameStateHistory = new Stack<GameState>();
        private readonly IMovesGenerator _movesGenerator;

        public GameStateUpdater(
            IMovesGenerator movesGenerator,
            IBuildMoveGenerator buildMoveGenerator,
            IGameOverEval gameOverEval
        )
        {
            _movesGenerator = movesGenerator;
            _buildMoveGenerator = buildMoveGenerator;
            _gameOverEval = gameOverEval;
        }

        public GameState RevertGameState() =>
            _gameStateHistory.Count > 1 ? _gameStateHistory.Pop() : _gameStateHistory.Peek();

        public void SetInitialGameState(GameState gameState)
        {
            _gameStateHistory.Push(gameState);
        }

        public GameState UpdateGameState(Position from, Position to, PieceColour turn)
        {
            var previousGameState = _gameStateHistory.Peek();
            var newBoardState = previousGameState.BoardState.WithMove(from, to, turn);
            return UpdateGameState(turn, newBoardState);
        }

        public GameState UpdateGameState(Position buildPosition, PieceType piece, PieceColour turn)
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

        private static PieceColour NextTurn(PieceColour turn) =>
            turn == PieceColour.White ? PieceColour.Black : PieceColour.White;

        private void ResolveBuilds(BoardState boardState, PieceColour turn)
        {
            var activeBuildPositions = boardState.ActiveBuilds.ToArray();

            for (var i = 0; i < activeBuildPositions.Length; i++)
            {
                var pos = activeBuildPositions[i];
                ref var tile = ref boardState.GetTileAt(pos);
                var canBuild = tile.BuildTileState.Turns == 0 &&
                               tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                               tile.CurrentPiece == PieceType.NullPiece &&
                               tile.BuildTileState.BuildingPiece.Colour() == turn;
                if (canBuild)
                {
                    // Account for new active piece in active builds and pieces
                    boardState.ActiveBuilds.Remove(tile.Position);
                    boardState.ActivePieces.Add(tile.Position);

                    // tile.CurrentPiece = tile.BuildTileState.BuildingPiece;
                    // tile.BuildTileState = new BuildTileState(); // reset build state
                }
            }
        }

        private PlayerState CalculateBuildPoints(PieceColour pieceColour, BoardState boardState,
            int maxPoints)
        {
            var result = 0;
            var activeBuilds = boardState.ActiveBuilds;
            var activePieces = boardState.ActivePieces;
            var activePositions = activeBuilds.Union(activePieces);

            foreach (var pos in activePositions)
            {
                ref var tile = ref boardState.GetTileAt(pos);
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
    }
}