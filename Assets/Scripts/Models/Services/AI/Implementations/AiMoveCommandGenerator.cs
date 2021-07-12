using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Models.Services.AI.Interfaces;
using Models.Services.Build.Interfaces;
using Models.Services.Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;

namespace Models.Services.AI
{
    public class AiMoveCommandGenerator : IAiCommandGenerator
    {
        private readonly IPieceMover _pieceMover;
        private readonly IBuilder _builder;
        private readonly IGameStateUpdater _gameStateUpdater;

        public AiMoveCommandGenerator(IPieceMover pieceMover, IGameStateUpdater gameStateUpdater, IBuilder builder)
        {
            _pieceMover = pieceMover;
            _gameStateUpdater = gameStateUpdater;
            _builder = builder;
        }

        public IEnumerable<Func<BoardState, PieceColour, GameState>> GenerateCommands(GameState gameState) =>
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