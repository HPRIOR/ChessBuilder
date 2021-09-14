using System.Collections.Generic;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.Board;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Controllers.PieceMovers
{
    [TestFixture]
    public class PieceMoverTest : ZenjectUnitTestFixture
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
        }

        private IPieceMover _pieceMover;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            PieceMoverInstaller.Install(Container);
            BuildResolverInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            BuildStateDecrementorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _pieceMover = Container.Resolve<IPieceMover>();
        }


        [Test]
        public void EvacuatedTilePieceIsNull()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            Assert.AreNotEqual(PieceType.NullPiece, board[1, 1].CurrentPiece.Type);

            var boardState = new BoardState(board);

            _pieceMover.ModifyBoardState(boardState, new Position(1, 1), new Position(2, 2));
            Assert.AreEqual(PieceType.NullPiece, boardState.Board[1, 1].CurrentPiece.Type);
        }

        [Test]
        public void PieceInTileIsDisplacedOnMove()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[2, 2].CurrentPiece = new Piece(PieceType.WhiteKing);

            var boardState = new BoardState(board);

            _pieceMover.ModifyBoardState(boardState, new Position(1, 1), new Position(2, 2));
            Assert.AreEqual(PieceType.BlackKing, boardState.Board[2, 2].CurrentPiece.Type);
            Assert.AreEqual(PieceType.NullPiece, boardState.Board[1, 1].CurrentPiece.Type);
        }


        [Test]
        public void TileContainsNewPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            var boardState = new BoardState(board);

            var oldPiece = board[1, 1].CurrentPiece;

            _pieceMover.ModifyBoardState(boardState, new Position(1, 1), new Position(2, 2));

            var newPiece = boardState.Board[2, 2].CurrentPiece;
            Assert.AreNotSame(oldPiece, newPiece);
        }

        [Test]
        public void ActivePiecesModified()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            var boardState = new BoardState(board);

            Assert.That(boardState.ActivePieces, Is.EquivalentTo(new HashSet<Position> { new Position(1, 1) }));
            _pieceMover.ModifyBoardState(boardState, new Position(1, 1), new Position(2, 2));

            Assert.That(boardState.ActivePieces, Is.EquivalentTo(new HashSet<Position> { new Position(2, 2) }));
        }


        [Test]
        public void ActiveWhitePiecesModified()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            var boardState = new BoardState(board);

            Assert.That(boardState.ActiveWhitePieces, Is.EquivalentTo(new HashSet<Position> { new Position(1, 1) }));
            _pieceMover.ModifyBoardState(boardState, new Position(1, 1), new Position(2, 2));

            Assert.That(boardState.ActiveWhitePieces, Is.EquivalentTo(new HashSet<Position> { new Position(2, 2) }));
        }

        [Test]
        public void ActiveBlackPiecesModified()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            var boardState = new BoardState(board);

            Assert.That(boardState.ActiveBlackPieces, Is.EquivalentTo(new HashSet<Position> { new Position(1, 1) }));
            _pieceMover.ModifyBoardState(boardState, new Position(1, 1), new Position(2, 2));

            Assert.That(boardState.ActiveBlackPieces, Is.EquivalentTo(new HashSet<Position> { new Position(2, 2) }));
        }
    }
}