using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Models.Services.Board;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.BoardTests
{
    [TestFixture]
    public class BuildStateDecrementorTests : ZenjectUnitTestFixture
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

        private void InstallBindings()
        {
            BoardGeneratorInstaller.Install(Container);
            BuildStateDecrementorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void EmptyBoardWillNotChange()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board);
            BuildStateDecrementor.DecrementBuilds(boardState);
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                var tile = board[i][j];

                Assert.That(tile.BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
                Assert.That(tile.BuildTileState.Turns, Is.EqualTo(0));
            }
        }

        [Test]
        public void NonEmptyBoard_WithNoBuilds_WillNotChange()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1][1].CurrentPiece = PieceType.WhiteKing;
            board[7][7].CurrentPiece = PieceType.BlackKing;
            var boardState = new BoardState(board);
            BuildStateDecrementor.DecrementBuilds(boardState);
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                var tile = board[i][j];
                Assert.That(tile.BuildTileState.BuildingPiece, Is.EqualTo(PieceType.NullPiece));
                Assert.That(tile.BuildTileState.Turns, Is.EqualTo(0));
            }
        }

        [Test]
        public void NonEmptyBoard_WithBuilds_WillBeDecremented()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1][1].CurrentPiece = PieceType.WhiteKing;
            board[7][7].CurrentPiece = PieceType.BlackKing;
            board[6][6].BuildTileState = new BuildTileState(PieceType.WhiteQueen);
            var boardState = new BoardState(board);
            BuildStateDecrementor.DecrementBuilds(boardState);

            var sut = boardState.GetTileAt(6,6).BuildTileState;
            Assert.That(sut.Turns, Is.EqualTo(8));
            Assert.That(sut.BuildingPiece, Is.EqualTo(PieceType.WhiteQueen));
        }
    }
}