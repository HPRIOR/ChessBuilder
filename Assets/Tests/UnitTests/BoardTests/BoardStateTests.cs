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

        private BoardState GetBoardState()
        {
            return new BoardState();
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

            Assert.IsNotNull(boardState.Board[0, 0]);
        }

        [Test]
        public void BoardIsCorrectSize()
        {
            var boardState = GetBoardState();

            Assert.DoesNotThrow(() =>
                {
                    Debug.Log(boardState.Board[7, 7].CurrentPiece);
                    Debug.ClearDeveloperConsole();
                }
            );
        }

        [Test]
        public void BoardContainsCorrectBoardPositions(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(new BoardPosition(x, y), boardState.Board[x, y].BoardPosition);
        }

        [Test]
        public void GetterRetrievesCorrectTile(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(boardState.Board[x, y].BoardPosition, new BoardPosition(x, y));
        }

        [Test]
        public void BoardContainsNoPiecesOnInit(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(new Piece(PieceType.NullPiece), boardState.Board[x, y].CurrentPiece);
        }

        [Test]
        public void TilesAreAtCorrectPositions(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            var boardState = GetBoardState();
            Assert.AreEqual(boardState.Board[x, y].BoardPosition.Vector,
                new Vector2(x + 0.5f, y + 0.5f));
        }
    }
}