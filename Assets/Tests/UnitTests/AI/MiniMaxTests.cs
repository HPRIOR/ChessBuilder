using System.Collections.Generic;
using System.Collections.Immutable;
using Bindings.Installers.AIInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.AI;
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
    public class MiniMaxTests : ZenjectUnitTestFixture
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

        private IBoardGenerator _boardGenerator;
        private MiniMax _miniMax;

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
            MiniMaxInstaller.Install(Container);
            StaticEvaluatorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _miniMax = Container.Resolve<MiniMax>();
        }

        [Test]
        public void MoveIsGenerated()
        {
            var board = _boardGenerator.GenerateBoard();
            board[5, 3].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[5, 5].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[6, 6].CurrentPiece = new Piece(PieceType.BlackQueen);
            var boardState = new BoardState(board);
            var possibleMoves = new Dictionary<Position, HashSet<Position>>
            {
                {new Position(5, 5), new HashSet<Position> {new Position(6, 6), new Position(6, 5)}}
            }.ToImmutableDictionary(x => x.Key, x => x.Value.ToImmutableHashSet());

            var gameState = new GameState(false, false, new PlayerState(0), new PlayerState(0), possibleMoves,
                new BuildMoves(ImmutableHashSet<Position>.Empty, ImmutableHashSet<PieceType>.Empty), boardState);

            var result = _miniMax.GetMaximizingTurn(gameState, 10, PieceColour.White, true);
            Debug.Log(result.move);
            Debug.Log(result.score);

        }
    }
}