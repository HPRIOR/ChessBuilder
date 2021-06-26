using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
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
            var boardState = new BoardState();
            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));
            Assert.AreNotSame(newState, boardState);
        }


        [Test]
        public void NewBoardIsDeepCopy()
        {
            var boardState = new BoardState();
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
            var boardState = new BoardState(board);
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
            var boardState = new BoardState(board);
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
            var boardState = new BoardState(board);

            var oldPiece = board[1, 1].CurrentPiece;

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));

            var newPiece = newState.Board[2, 2].CurrentPiece;
            Assert.AreNotSame(oldPiece, newPiece);
        }


        [Test]
        public void BuildStateIsDecremented()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 5].BuildState = new BuildState(5, PieceType.BlackKnight);
            var boardState = new BoardState(board);

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));

            var buildState = newState.Board[5, 5].BuildState;

            Assert.That(buildState.Turns, Is.EqualTo(4));
        }


        [Test]
        public void BuildStateIsResolved()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 5].BuildState = new BuildState(1, PieceType.BlackKnight);
            var boardState = new BoardState(board);

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));


            Assert.That(newState.Board[5, 5].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKnight));
        }

        [Test]
        public void BuildStateIsResetAfterResolved()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 5].BuildState = new BuildState(1, PieceType.BlackKnight);
            var boardState = new BoardState(board);

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));


            Assert.That(newState.Board[5, 5].BuildState, Is.EqualTo(new BuildState()));
        }


        [Test]
        public void WhenBuildIsBlocked_NoPieceIsBuilt()
        {
            var board = _boardGenerator.GenerateBoard();
            board[5, 5].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 5].BuildState = new BuildState(1, PieceType.BlackKnight);
            var boardState = new BoardState(board);

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));

            Assert.That(newState.Board[5, 5].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKing));
        }


        [Test]
        public void WhenBuildIsBlocked_BuildStateIsDecremented()
        {
            var board = _boardGenerator.GenerateBoard();
            board[5, 5].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 5].BuildState = new BuildState(1, PieceType.BlackKnight);
            var boardState = new BoardState(board);

            var newState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));

            Assert.That(newState.Board[5, 5].BuildState,
                Is.EqualTo(new BuildState(0, PieceType.BlackKnight)));
        }

        [Test]
        public void BuildIsUnblocked_ByMovingPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[5, 5].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 5].BuildState = new BuildState(1, PieceType.BlackKnight);
            var boardState = new BoardState(board);

            // update state but don't move blocking piece
            var firstState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(1, 1), new Position(2, 2));


            Assert.That(firstState.Board[5, 5].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKing));

            // move blocking piece
            var secondState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(5, 5), new Position(6, 6));

            Assert.That(secondState.Board[5, 5].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKnight));
        }

        [Test]
        public void BuildIsBlocked_ByMovingPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5, 5].BuildState = new BuildState(1, PieceType.BlackKnight);
            var boardState = new BoardState(board);

            var firstState =
                _pieceMover.GenerateNewBoardState(boardState, new Position(4, 4), new Position(5, 5));

            Assert.That(firstState.Board[5, 5].CurrentPiece.Type, Is.EqualTo(PieceType.BlackKing));
        }
    }
}