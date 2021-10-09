using System.Collections.Generic;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Models.Services.Board;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.BuildMoves
{
    [TestFixture]
    public class BuilderTests : ZenjectUnitTestFixture
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

        private IBuilder _builder;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BuilderInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            BuildResolverInstaller.Install(Container);
            BuildStateDecrementorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _builder = Container.Resolve<IBuilder>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }


        [Test]
        public void Updates_WithBuildStateAtPosition()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);
            _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.That(boardState.Board[5][5].BuildTileState,
                Is.EqualTo(new BuildTileState(3, PieceType.BlackKnight)));
        }

        [Test]
        public void OverwritesPreviousBuild()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);
            boardState.Board[5][5].BuildTileState = new BuildTileState(PieceType.BlackPawn);
            _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.That(boardState.Board[5][5].BuildTileState,
                Is.EqualTo(new BuildTileState(3, PieceType.BlackKnight)));
        }

        [Test]
        public void OverwritesBuild_DueNextTurn()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);
            boardState.Board[5][5].BuildTileState = new BuildTileState(PieceType.BlackPawn);

            _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.That(boardState.Board[5][5].BuildTileState,
                Is.EqualTo(new BuildTileState(3, PieceType.BlackKnight)));
            Assert.That(boardState.Board[5][5].CurrentPiece,
                Is.EqualTo(PieceType.NullPiece));
        }

        [Test]
        public void UpdatesActiveBuilds()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);
            boardState.Board[5][5].BuildTileState = new BuildTileState(PieceType.BlackPawn);

            _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.That(boardState.ActiveBuilds, Is.EquivalentTo(new HashSet<Position> { new Position(5, 5) }));
        }
    }
}