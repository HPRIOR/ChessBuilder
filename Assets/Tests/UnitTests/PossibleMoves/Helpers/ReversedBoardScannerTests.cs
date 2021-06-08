using System.Collections.Generic;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.PieceInstallers;
using Game.Implementations;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class ReversedBoardScannerTests : ZenjectUnitTestFixture
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

        private IBoardScannerFactory _boardScannerFactory;
        private BoardSetup _boardSetup;

        private void InstallBindings()
        {
            BoardScannerInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardSetupInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardScannerFactory = Container.Resolve<IBoardScannerFactory>();
            _boardSetup = Container.Resolve<BoardSetup>();
        }

        [Test]
        public void WhiteScanner_StopsOnFriendlyPiece()
        {
            var scanner = _boardScannerFactory.Create(PieceColour.White, false);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var positions = scanner.ScanIn(Direction.N, new BoardPosition(4, 0), boardState);
            var expected = new List<BoardPosition>
            {
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3),
                new BoardPosition(4, 4)
            };

            Assert.That(positions, Is.EquivalentTo(expected));
        }

        [Test]
        public void WhiteScanner_StopsBeforeOpposingPiece()
        {
            var scanner = _boardScannerFactory.Create(PieceColour.White, false);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(4, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var positions = scanner.ScanIn(Direction.N, new BoardPosition(4, 0), boardState);
            var expected = new List<BoardPosition>
            {
                new BoardPosition(4, 1),
                new BoardPosition(4, 2),
                new BoardPosition(4, 3)
            };

            Assert.That(positions, Is.EquivalentTo(expected));
        }


        [Test]
        public void BlackScanner_StopsOnFriendlyPiece()
        {
            var scanner = _boardScannerFactory.Create(PieceColour.Black, false);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.BlackKing, new BoardPosition(3, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var positions = scanner.ScanIn(Direction.N, new BoardPosition(4, 0), boardState);
            var expected = new List<BoardPosition>
            {
                new BoardPosition(3, 4),
                new BoardPosition(3, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(positions, Is.EquivalentTo(expected));
        }

        [Test]
        public void BlackScanner_StopsBeforeOpposingPiece()
        {
            var scanner = _boardScannerFactory.Create(PieceColour.Black, false);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (PieceType.WhiteKing, new BoardPosition(3, 4))
            };

            var boardState = _boardSetup.SetupBoardWith(pieces);

            var positions = scanner.ScanIn(Direction.N, new BoardPosition(4, 0), boardState);
            var expected = new List<BoardPosition>
            {
                new BoardPosition(3, 5),
                new BoardPosition(3, 6)
            };

            Assert.That(positions, Is.EquivalentTo(expected));
        }
    }
}