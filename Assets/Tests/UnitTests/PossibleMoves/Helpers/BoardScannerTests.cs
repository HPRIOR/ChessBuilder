using System.Collections.Generic;
using System.Linq;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class BoardScannerTests : ZenjectUnitTestFixture
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
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            BoardScannerInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardScannerFactory = Container.Resolve<IBoardScannerFactory>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void OnEmptyBoard_WithWhitePiece_OnSWCorner_ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White);
            var board = new BoardState(_boardGenerator);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(4, 4),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(7, 7)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void OnEmptyBoard_WithBlackPiece_OnNECorner__ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black);
            var board = new BoardState(_boardGenerator);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(4, 4),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void WithEnemy_OnSWCorner_WithBlackPiece_OnNECorner_ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black);
            var board = new BoardState(_boardGenerator);
            board.Board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKnight);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(0, 0),
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(4, 4),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithEnemy_OnNECorner_WithWhitePiece_OnSWCorner_ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White);
            var board = new BoardState(_boardGenerator);
            board.Board[7, 7].CurrentPiece = new Piece(PieceType.BlackBishop);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(4, 4),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6),
                new BoardPosition(7, 7)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithFriend_OnNECorner_WithWhitePiece_OnSWCorner_ScannerGetsAllPositionsMinusOne()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White);
            var board = new BoardState(_boardGenerator);
            board.Board[7, 7].CurrentPiece = new Piece(PieceType.WhiteBishop);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(4, 4),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithFriend_OnSWCorner_WithBlackPiece_OnNECorner_ScannerGetsAllPositionsMinusOne()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black);
            var board = new BoardState(_boardGenerator);
            board.Board[0, 0].CurrentPiece = new Piece(PieceType.BlackKnight);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(4, 4),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6)
            };
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithFriend_InMiddle_WithWhitePiece_OnSWCorner_ScannerIsBlocked()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White);
            var board = new BoardState(_boardGenerator);
            board.Board[4, 4].CurrentPiece = new Piece(PieceType.WhiteBishop);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void WithFriend_InMiddle_WithBlackPiece_OnNECorner_ScannerIsBlocked()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black);
            var board = new BoardState(_boardGenerator);
            board.Board[4, 4].CurrentPiece = new Piece(PieceType.BlackKnight);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(5, 5),
                new BoardPosition(6, 6)
            };
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithEnemy_InMiddle_WithWhitePiece_OnSWCorner_ScannerStopsOnFriend()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White);
            var board = new BoardState(_boardGenerator);
            board.Board[4, 4].CurrentPiece = new Piece(PieceType.BlackBishop);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(1, 1),
                new BoardPosition(2, 2),
                new BoardPosition(3, 3),
                new BoardPosition(4, 4)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithEnemy_InMiddle_WithBlackPiece_OnNECorner_ScannerStopsOnFriend()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black);
            var board = new BoardState(_boardGenerator);
            board.Board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKnight);
            var positions = boardScanner.ScanIn(Direction.NE, new BoardPosition(0, 0), board);
            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(4, 4),
                new BoardPosition(5, 5),
                new BoardPosition(6, 6)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void OnEmptyBoard_WithWhitePiece_OnBottom_ScannerGetAll(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x
        )
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White);
            var board = new BoardState(_boardGenerator);
            var positions = boardScanner.ScanIn(Direction.N, new BoardPosition(x, 0), board);

            var expected = new HashSet<IBoardPosition>
            {
                new BoardPosition(x, 1),
                new BoardPosition(x, 2),
                new BoardPosition(x, 3),
                new BoardPosition(x, 4),
                new BoardPosition(x, 5),
                new BoardPosition(x, 6),
                new BoardPosition(x, 7)
            };
            Assert.AreEqual(expected.Count(), positions.Count());
            foreach (var boardPosition in positions) Assert.AreEqual(true, expected.Contains(boardPosition));
        }
    }
}