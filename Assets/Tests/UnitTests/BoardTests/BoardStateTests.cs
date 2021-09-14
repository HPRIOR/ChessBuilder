using System.Collections.Generic;
using Bindings.Installers.ModelInstallers.Board;
using Models.Services.Board;
using Models.State.Board;
using Models.State.BuildState;
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
            Assert.AreEqual(PieceType.NullPiece, boardState.Board[1, 1].BuildTileState.BuildingPiece);
        }

        [Test]
        public void BoardContains_BuildStateWithZeroTurns()
        {
            var boardState = GetBoardState();
            Assert.AreEqual(0, boardState.Board[1, 1].BuildTileState.Turns);
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

        [Test]
        public void ActivePiecesAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[2, 2].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[3, 3].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackBishop);
            var boardState = new BoardState(board);

            var expected = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4)
            };
            Assert.That(boardState.ActivePieces, Is.EquivalentTo(expected));
        }

        [Test]
        public void ActiveWhitePiecesAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[2, 2].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[3, 3].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteBishop);
            var boardState = new BoardState(board);

            var expected = new HashSet<Position>
            {
                new Position(3, 3),
                new Position(4, 4)
            };
            Assert.That(boardState.ActiveWhitePieces, Is.EquivalentTo(expected));
        }

        [Test]
        public void ActiveBlackPiecesAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[2, 2].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[3, 3].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteBishop);
            var boardState = new BoardState(board);

            var expected = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(2, 2)
            };
            Assert.That(boardState.ActiveBlackPieces, Is.EquivalentTo(expected));
        }

        [Test]
        public void ActiveBuildsAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(PieceType.BlackBishop);
            board[2, 2].BuildTileState = new BuildTileState(PieceType.BlackBishop);
            board[3, 3].BuildTileState = new BuildTileState(PieceType.WhiteBishop);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhiteBishop);
            var boardState = new BoardState(board);

            var expected = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(2, 2),
                new Position(3, 3),
                new Position(4, 4)
            };
            Assert.That(boardState.ActiveBuilds, Is.EquivalentTo(expected));
        }

        [Test]
        public void ActiveBlackBuildsAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(PieceType.BlackBishop);
            board[2, 2].BuildTileState = new BuildTileState(PieceType.BlackBishop);
            board[3, 3].BuildTileState = new BuildTileState(PieceType.WhiteBishop);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhiteBishop);
            var boardState = new BoardState(board);

            var expected = new HashSet<Position>
            {
                new Position(1, 1),
                new Position(2, 2)
            };
            Assert.That(boardState.ActiveBlackBuilds, Is.EquivalentTo(expected));
        }

        [Test]
        public void ActiveWhiteBuildsAreCorrect()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].BuildTileState = new BuildTileState(PieceType.BlackBishop);
            board[2, 2].BuildTileState = new BuildTileState(PieceType.BlackBishop);
            board[3, 3].BuildTileState = new BuildTileState(PieceType.WhiteBishop);
            board[4, 4].BuildTileState = new BuildTileState(PieceType.WhiteBishop);
            var boardState = new BoardState(board);

            var expected = new HashSet<Position>
            {
                new Position(3, 3),
                new Position(4, 4)
            };
            Assert.That(boardState.ActiveWhiteBuilds, Is.EquivalentTo(expected));
        }
    }
}