using System.Collections.Generic;
using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Tests.UnitTests.PossibleMoves.PieceMoves.Utils;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class CheckStateTests : ZenjectUnitTestFixture
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

        private IBoardGenerator _boardGenerator;
        private IPossibleMoveFactory _possibleMoveFactory;

        private void InstallBindings()
        {
            PossibleMovesBinder.InstallBindings(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _possibleMoveFactory = Container.Resolve<IPossibleMoveFactory>();
        }

        [Test]
        public void WhenNotInCheck_PropertyIsFalse()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[6, 5].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            ICheckedState checkedState =
                new CheckedState(boardState, new BoardPosition(6, 5), _possibleMoveFactory);

            Assert.IsFalse(checkedState.IsTrue);
        }

        [Test]
        public void WhenInCheck_PropertyIsTrue()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackQueen);

            var boardState = new BoardState(board);

            ICheckedState checkedState =
                new CheckedState(boardState, new BoardPosition(7, 7), _possibleMoveFactory);

            Assert.IsTrue(checkedState.IsTrue);
        }

        [Test]
        public void WhenInCheck_OnlyInterceptingMovesAreGiven()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[3, 5].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackQueen);

            var boardState = new BoardState(board);

            ICheckedState checkedState =
                new CheckedState(boardState, new BoardPosition(7, 7), _possibleMoveFactory);

            var possibleMoves = new Dictionary<BoardPosition, HashSet<BoardPosition>>
            {
                {
                    new BoardPosition(3, 5), new HashSet<BoardPosition>
                    {
                        new BoardPosition(3, 6),
                        new BoardPosition(2, 7),
                        new BoardPosition(4, 6),
                        new BoardPosition(5, 7),
                        new BoardPosition(4, 7),
                        new BoardPosition(3, 3),
                        new BoardPosition(4, 4),
                        new BoardPosition(5, 5)
                    }
                },
                {new BoardPosition(0, 0), new HashSet<BoardPosition>()}
            };

            var nonTurnMoves = new Dictionary<BoardPosition, HashSet<BoardPosition>>();

            var interceptingMoves =
                checkedState.PossibleMovesWhenInCheck(possibleMoves, nonTurnMoves, new BoardPosition(0, 0));
            var expected = new Dictionary<BoardPosition, HashSet<BoardPosition>>
            {
                {
                    new BoardPosition(3, 5), new HashSet<BoardPosition>
                    {
                        new BoardPosition(3, 3),
                        new BoardPosition(4, 4),
                        new BoardPosition(5, 5)
                    }
                }
            };
            Assert.AreEqual(expected[new BoardPosition(3, 5)], interceptingMoves[new BoardPosition(3, 5)]);
        }
        // only king can move if two checks occur

        // king can move in front of pawn

        // king cannot move to either side of king
    }
}