using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Models.Services.Build.Interfaces;
using Models.Services.Interfaces;
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
        }

        private void ResolveContainer()
        {
            _builder = Container.Resolve<IBuilder>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void CreatesNewBoardState()
        {
            var boardState = new BoardState();
            var newBoardState = _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.AreNotSame(boardState, newBoardState);
        }


        [Test]
        public void CreatesNewBoardState_WithBuilddStateAtPosition()
        {
            var boardState = new BoardState();
            var newBoardState = _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.That(newBoardState.Board[5, 5].BuildTileState,
                Is.EqualTo(new BuildTileState(3, PieceType.BlackKnight)));
        }

        [Test]
        public void OverwritesPreviousBuild()
        {
            var boardState = new BoardState();
            boardState.Board[5, 5].BuildTileState = new BuildTileState(PieceType.BlackPawn);
            var newBoardState = _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.That(newBoardState.Board[5, 5].BuildTileState,
                Is.EqualTo(new BuildTileState(3, PieceType.BlackKnight)));
        }

        [Test]
        public void DecrementsBuildsOnBoard()
        {
            var boardState = new BoardState();
            boardState.Board[5, 5].BuildTileState = new BuildTileState(PieceType.WhiteRook);

            var newBoardState = _builder.GenerateNewBoardState(boardState, new Position(6, 6), PieceType.BlackKnight);

            Assert.That(newBoardState.Board[5, 5].BuildTileState,
                Is.EqualTo(new BuildTileState(4, PieceType.WhiteRook)));
        }


        [Test]
        public void OverwritesBuild_DueNextTurn()
        {
            var boardState = new BoardState();
            boardState.Board[5, 5].BuildTileState = new BuildTileState(PieceType.BlackPawn);

            var newBoardState = _builder.GenerateNewBoardState(boardState, new Position(5, 5), PieceType.BlackKnight);

            Assert.That(newBoardState.Board[5, 5].BuildTileState,
                Is.EqualTo(new BuildTileState(3, PieceType.BlackKnight)));
            Assert.That(newBoardState.Board[5, 5].CurrentPiece.Type,
                Is.EqualTo(PieceType.NullPiece));
        }
    }
}