using Bindings.Installers.BoardInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
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

        private IBoardState GetBoardState()
        {
            return new BoardState(_boardGenerator.GenerateBoard());
        }

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

            Assert.IsNotNull(boardState.GetTileAt(new BoardPosition(0, 0)));
        }

        [Test]
        public void BoardIsCorrectSize()
        {
            var boardState = GetBoardState();

            Assert.DoesNotThrow(() => boardState.GetTileAt(new BoardPosition(7, 7)));
        }

        [Test]
        public void BoardContainsCorrectBoardPositions(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(new BoardPosition(x, y), boardState.GetTileAt(new BoardPosition(x, y)).BoardPosition);
        }

        [Test]
        public void GetterRetrievesCorrectTile(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(boardState.GetTileAt(new BoardPosition(x, y)).BoardPosition, new BoardPosition(x, y));
        }

        [Test]
        public void BoardContainsNoPiecesOnInit(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(new Piece(PieceType.NullPiece), boardState.GetTileAt(new BoardPosition(x, y)).CurrentPiece);
        }

        [Test]
        public void TilesAreAtCorrentPositions(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(boardState.GetTileAt(new BoardPosition(x, y)).BoardPosition.Vector,
                new Vector2(x + 0.5f, y + 0.5f));
        }


        [Test]
        public void BoardRotatesCorrectly()
        {
            var boardState = GetBoardState();

            Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(0, 0)),
                boardState.GetTileAt(new BoardPosition(7, 7)));
            Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(1, 1)),
                boardState.GetTileAt(new BoardPosition(6, 6)));
            Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(4, 6)),
                boardState.GetTileAt(new BoardPosition(3, 1)));
            Assert.AreEqual(boardState.GetMirroredTileAt(new BoardPosition(3, 2)),
                boardState.GetTileAt(new BoardPosition(4, 5)));
        }
    }
}