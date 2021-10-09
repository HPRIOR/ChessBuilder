using System.Collections.Generic;
using System.Linq;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
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
        }

        [Test]
        public void OnEmptyBoard_WithWhitePiece_OnSWCorner_ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White, Turn.Turn);
            var board = new BoardState();
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5),
                new Position(6, 6),
                new Position(7, 7)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void OnEmptyBoard_WithBlackPiece_OnNECorner__ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black, Turn.Turn);
            var board = new BoardState();
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5),
                new Position(6, 6)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void WithEnemy_OnSWCorner_WithBlackPiece_OnNECorner_ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black, Turn.Turn);
            var board = new BoardState();
            board.Board[0][0].CurrentPiece = PieceType.WhiteKnight;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(0, 0),
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5),
                new Position(6, 6)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithEnemy_OnNECorner_WithWhitePiece_OnSWCorner_ScannerGetsAllPositions()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White, Turn.Turn);
            var board = new BoardState();
            board.Board[7][7].CurrentPiece = PieceType.BlackBishop;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5),
                new Position(6, 6),
                new Position(7, 7)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithFriend_OnNECorner_WithWhitePiece_OnSWCorner_ScannerGetsAllPositionsMinusOne()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White, Turn.Turn);
            var board = new BoardState();
            board.Board[7][7].CurrentPiece = PieceType.WhiteBishop;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5),
                new Position(6, 6)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithFriend_OnSWCorner_WithBlackPiece_OnNECorner_ScannerGetsAllPositionsMinusOne()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black, Turn.Turn);
            var board = new BoardState();
            board.Board[0][0].CurrentPiece = PieceType.BlackKnight;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5),
                new Position(6, 6)
            };
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithFriend_InMiddle_WithWhitePiece_OnSWCorner_ScannerIsBlocked()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White, Turn.Turn);
            var board = new BoardState();
            board.Board[4][4].CurrentPiece = PieceType.WhiteBishop;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void WithFriend_InMiddle_WithBlackPiece_OnNECorner_ScannerIsBlocked()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black, Turn.Turn);
            var board = new BoardState();
            board.Board[4][4].CurrentPiece = PieceType.BlackKnight;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(5, 5),
                new Position(6, 6)
            };
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithEnemy_InMiddle_WithWhitePiece_OnSWCorner_ScannerStopsOnFriend()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White, Turn.Turn);
            var board = new BoardState();
            board.Board[4][4].CurrentPiece = PieceType.BlackBishop;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }


        [Test]
        public void WithEnemy_InMiddle_WithBlackPiece_OnNECorner_ScannerStopsOnFriend()
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.Black, Turn.Turn);
            var board = new BoardState();
            board.Board[4][4].CurrentPiece = PieceType.WhiteKnight;
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.Ne, new Position(0, 0), board, result);
            var expected = new List<Position>
            {
                new Position(4, 4),
                new Position(5, 5),
                new Position(6, 6)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }

        [Test]
        public void OnEmptyBoard_WithWhitePiece_OnBottom_ScannerGetAll(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x
        )
        {
            var boardScanner = _boardScannerFactory.Create(PieceColour.White, Turn.Turn);
            var board = new BoardState();
            var result = new List<Position>();
            boardScanner.ScanIn(Direction.N, new Position(x, 0), board, result);

            var expected = new List<Position>
            {
                new Position(x, 1),
                new Position(x, 2),
                new Position(x, 3),
                new Position(x, 4),
                new Position(x, 5),
                new Position(x, 6),
                new Position(x, 7)
            };
            Assert.AreEqual(expected.Count(), result.Count());
            foreach (var boardPosition in result) Assert.AreEqual(true, expected.Contains(boardPosition));
        }
    }
}