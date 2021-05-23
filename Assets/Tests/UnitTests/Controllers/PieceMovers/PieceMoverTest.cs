using Bindings.Installers.BoardInstallers;
using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
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
            BoardGeneratorInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _pieceMover = Container.Resolve<IPieceMover>();
        }

        [Test]
        public void GeneratesNewBoard()
        {
            var boardState = new BoardState(_boardGenerator);
            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new BoardPosition(1, 1), new BoardPosition(2, 2));
            Assert.AreNotSame(newState, boardState);
        }

        [Test]
        public void EvacuatedTileIsNull()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            Assert.AreNotEqual(PieceType.NullPiece, board[1, 1].CurrentPiece.Type);
            var boardState = new BoardState(board);
            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new BoardPosition(1, 1), new BoardPosition(2, 2));
            Assert.AreEqual(PieceType.NullPiece, newState.Board[1, 1].CurrentPiece.Type);
        }

        [Test]
        public void TileIsDisplacedOnMove()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[2, 2].CurrentPiece = new Piece(PieceType.WhiteKing);
            var boardState = new BoardState(board);
            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new BoardPosition(1, 1), new BoardPosition(2, 2));
            Assert.AreEqual(PieceType.BlackKing, newState.Board[2, 2].CurrentPiece.Type);
            Assert.AreEqual(PieceType.NullPiece, newState.Board[1, 1].CurrentPiece.Type);
        }
    }
}