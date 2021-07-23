using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Controllers.Interfaces;
using Models.Services.AI.Interfaces;
using Models.Services.Build.Interfaces;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public class AiMoveGenerator : IAiMoveGenerator
    {
        private readonly IBuilder _builder;
        private readonly IGameStateUpdater _gameStateUpdater;
        private readonly IPieceMover _pieceMover;

        public AiMoveGenerator(IPieceMover pieceMover, IGameStateUpdater gameStateUpdater, IBuilder builder)
        {
            _pieceMover = pieceMover;
            _gameStateUpdater = gameStateUpdater;
            _builder = builder;
        }

        public IEnumerable<Func<BoardState, PieceColour, GameState>> GenerateMoves(GameState gameState) =>
            GetBuildCommands(gameState.PossibleBuildMoves)
                .Concat(GetMoveCommands(gameState.PossiblePieceMoves));

        private IEnumerable<Func<BoardState, PieceColour, GameState>> GetMoveCommands(
            ImmutableDictionary<Position, ImmutableHashSet<Position>> moves
        ) =>
            moves.SelectMany(moveSet =>
                moveSet.Value.Select(move =>
                {
                    Func<BoardState, PieceColour, GameState> command = (boardState, turn) =>
                    {
                        var newBoardState = _pieceMover.GenerateNewBoardState(boardState, moveSet.Key, move);
                        var nextTurn = turn == PieceColour.Black ? PieceColour.White : PieceColour.Black;
                        return _gameStateUpdater.UpdateGameState(newBoardState, nextTurn);
                    };
                    return command;
                }));


        private IEnumerable<Func<BoardState, PieceColour, GameState>> GetBuildCommands(BuildMoves builds) =>
            builds.BuildPositions.SelectMany(position =>
                builds.BuildPieces.Select(piece =>
                {
                    Func<BoardState, PieceColour, GameState> command = (boardState, turn) =>
                    {
                        var newBoardState = _builder.GenerateNewBoardState(boardState, position, piece);
                        var nextTurn = turn == PieceColour.Black ? PieceColour.White : PieceColour.Black;
                        return _gameStateUpdater.UpdateGameState(newBoardState, nextTurn);
                    };
                    return command;
                }));
    }
}