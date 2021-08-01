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
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _pieceMover = Container.Resolve<IPieceMover>();
        }

        [Test]
        public void GeneratesNewBoard()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board, new HashSet<Position>(), new HashSet<Position>());
            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));
            Assert.AreNotSame(newState, boardState);
        }


        [Test]
        public void NewBoardIsDeepCopy()
        {
            var board = _boardGenerator.GenerateBoard();
            var boardState = new BoardState(board, new HashSet<Position>(), new HashSet<Position>());
            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));
            Assert.AreNotSame(newState, boardState);
            Assert.AreNotSame(newState.Board, boardState.Board);
            Assert.AreNotSame(newState.Board[1, 1], boardState.Board[1, 1]);
        }

        [Test]
        public void EvacuatedTilePieceIsNull()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            Assert.AreNotEqual(PieceType.NullPiece, board[1, 1].CurrentPiece.Type);

            var activePieces = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());
            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));
            Assert.AreEqual(PieceType.NullPiece, newState.Board[1, 1].CurrentPiece.Type);
        }

        [Test]
        public void PieceInTileIsDisplacedOnMove()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[2, 2].CurrentPiece = new Piece(PieceType.WhiteKing);

            var activePieces = new HashSet<Position> {new Position(1, 1), new Position(2, 2)};
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));
            Assert.AreEqual(PieceType.BlackKing, newState.Board[2, 2].CurrentPiece.Type);
            Assert.AreEqual(PieceType.NullPiece, newState.Board[1, 1].CurrentPiece.Type);
        }


        [Test]
        public void TileContainsNewPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            var activePieces = new HashSet<Position> {new Position(1, 1)};
            var boardState = new BoardState(board, activePieces, new HashSet<Position>());

            var oldPiece = board[1, 1].CurrentPiece;

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));

            var newPiece = newState.Board[2, 2].CurrentPiece;
            Assert.AreNotSame(oldPiece, newPiece);
        }
    }
}