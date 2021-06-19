using Bindings.Installers.BoardInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.UnitTests.BoardTests
{
    [TestFixture]
    public class BoardStateTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Init()
        {
            BoardGeneratorInstaller.Install(Container);
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        private IBoardGenerator _boardGenerator;

        private BoardState GetBoardState() => new BoardState();

        [Test]
        public void BindsCorrectly()
        {
            var boardGenerator = Container.Resolve<IBoardGenerator>();

            Assert.NotNull(boardGenerator);
        }

        [Test]
        public void ResolvesToClass()
        {
            var boardState = GetBoardState();

            Assert.IsNotNull(boardState);
        }

        [Test]
        public void BoardIsGenerated()
        {
            var boardState = GetBoardState();

            Assert.IsNotNull(boardState.Board[0, 0]);
        }

        [Test]
        public void BoardIsCorrectSize()
        {
            var boardState = GetBoardState();

            Assert.AreEqual(64, boardState.Board.Length);
        }

        [Test]
        public void BoardContainsCorrectBoardPositions(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(new Position(x, y), boardState.Board[x, y].Position);
        }

        [Test]
        public void GetterRetrievesCorrectTile(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(boardState.Board[x, y].Position, new Position(x, y));
        }

        [Test]
        public void BoardContains_NoPiecesOnInit(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(new Piece(PieceType.NullPiece), boardState.Board[x, y].CurrentPiece);
        }

        [Test]
        public void BoardContains_BuildStateWithNullPiece()
        {
            var boardState = GetBoardState();
            Assert.AreEqual(PieceType.NullPiece, boardState.Board[1, 1].BuildState.BuildingPiece);
        }

        [Test]
        public void BoardContains_BuildStateWithZeroTurns()
        {
            var boardState = GetBoardState();
            Assert.AreEqual(0, boardState.Board[1, 1].BuildState.Turns);
        }

        [Test]
        public void TilesAreAtCorrectPositions(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(boardState.Board[x, y].Position.Vector,
                new Vector2(x + 0.5f, y + 0.5f));
        }
    }
}