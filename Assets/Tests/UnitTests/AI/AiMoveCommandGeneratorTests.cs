using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bindings.Installers.AIInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.AI.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;
using Models.State.PlayerState;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.UnitTests.AI
{
    [TestFixture]
    public class AiMoveCommandGeneratorTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Init()
        {
            InstallBindings();
            ResolveContainer();
        }

        [TearDown]
        public void TearDown()
        {
            Container.UnbindAll();
        }

        private IAiCommandGenerator _aiMoveCommandGenerator;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            AiMoveCommandGeneratorInstaller.Install(Container);
            PieceMoverInstaller.Install(Container);
            GameStateUpdaterInstaller.Install(Container);
            BuilderInstaller.Install(Container);
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);
            BuildPointsCalculatorInstaller.Install(Container);
            BuildResolverInstaller.Install(Container);
            GameOverEvalInstaller.Install(Container);
            MovesGeneratorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            PinnedPieceFilterInstaller.Install(Container);
            BoardInfoInstaller.Install(Container);
            MovesGeneratorRepositoryInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _aiMoveCommandGenerator = Container.Resolve<IAiCommandGenerator>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void GeneratesCorrectNumberOfCommands()
        {
            var board = _boardGenerator.GenerateBoard();
            board[2, 3].CurrentPiece = new Piece(PieceType.BlackPawn);
            var boardState = new BoardState(board);

            var moves = new Dictionary<Position, ImmutableHashSet<Position>>
            {
                {new Position(2, 2), new HashSet<Position> {new Position(2, 1)}.ToImmutableHashSet()}
            }.ToImmutableDictionary();

            var buildMoves = new BuildMoves(
                new HashSet<Position> {new Position(1, 1)}.ToImmutableHashSet(),
                new HashSet<PieceType> {PieceType.BlackPawn}.ToImmutableHashSet()
            );

            var gameState = new GameState(
                false,
                false,
                new PlayerState(10),
                new PlayerState(10),
                moves,
                buildMoves,
                boardState
            );
            var commands = _aiMoveCommandGenerator.GenerateCommands(gameState);
            Assert.That(commands.Count(), Is.EqualTo(2));
        }


        [Test]
        public void GeneratesCommands_WithCorrectPieceMoveOutput()
        {
            var board = _boardGenerator.GenerateBoard();
            board[2, 2].CurrentPiece = new Piece(PieceType.BlackPawn);
            var boardState = new BoardState(board);

            var moves = new Dictionary<Position, ImmutableHashSet<Position>>
            {
                {new Position(2, 2), new HashSet<Position> {new Position(2, 1)}.ToImmutableHashSet()}
            }.ToImmutableDictionary();

            var buildMoves = new BuildMoves(
                new HashSet<Position>().ToImmutableHashSet(),
                new HashSet<PieceType>().ToImmutableHashSet()
            );

            var gameState = new GameState(
                false,
                false,
                new PlayerState(10),
                new PlayerState(10),
                moves,
                buildMoves,
                boardState
            );
            var commands = _aiMoveCommandGenerator.GenerateCommands(gameState);
            var updatedGameState = commands.First()(gameState.BoardState, PieceColour.Black);
            var updatedBoard = updatedGameState.BoardState.Board;
            Assert.That(updatedBoard[2, 1].CurrentPiece.Type, Is.EqualTo(PieceType.BlackPawn));
        }

        [Test]
        public void GeneratesCommands_WithCorrectBuildMoveOutput()
        {
            var board = _boardGenerator.GenerateBoard();
            board[2, 2].CurrentPiece = new Piece(PieceType.BlackPawn);
            var boardState = new BoardState(board);

            var moves = new Dictionary<Position, ImmutableHashSet<Position>>()
                .ToImmutableDictionary();

            var buildMoves = new BuildMoves(
                new HashSet<Position> {new Position(5, 5)}.ToImmutableHashSet(),
                new HashSet<PieceType> {PieceType.WhitePawn}.ToImmutableHashSet()
            );

            var gameState = new GameState(
                false,
                false,
                new PlayerState(10),
                new PlayerState(10),
                moves,
                buildMoves,
                boardState
            );

            var commands = _aiMoveCommandGenerator.GenerateCommands(gameState);
            var updatedGameState = commands.First()(gameState.BoardState, PieceColour.White);
            var updatedBoard = updatedGameState.BoardState.Board;
            Assert.That(updatedBoard[5,5].BuildTileState.BuildingPiece, Is.EqualTo(PieceType.WhitePawn));
            Assert.That(updatedBoard[5,5].BuildTileState.Turns, Is.EqualTo(1));
        }
    }
}