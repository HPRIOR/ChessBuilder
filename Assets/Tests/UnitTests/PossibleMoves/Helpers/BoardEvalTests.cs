using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Tests.UnitTests.PossibleMoves.PieceMoves.Utils;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.Helpers
{
    [TestFixture]
    public class BoardEvalTests : ZenjectUnitTestFixture
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

        private IBoardEval _boardEval;
        private IBoardGenerator _boardGenerator;

        private void InstallBindings()
        {
            PossibleMovesBinder.InstallBindings(Container);
        }

        private void ResolveContainer()
        {
            _boardEval = Container.Resolve<IBoardEval>();
            _boardGenerator = Container.Resolve<IBoardGenerator>();
        }

        [Test]
        public void BoardEval_IsInstalled()
        {
            Assert.IsNotNull(_boardEval);
        }

        [Test]
        public void OnBlackTurn_MovesAreDividedBetweenTurn(
        )
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardEval.EvaluateBoard(boardState, PieceColour.Black);

            Assert.That(_boardEval.TurnMoves, Contains.Key(new BoardPosition(7, 7)));
            Assert.That(_boardEval.TurnMoves, Contains.Key(new BoardPosition(7, 6)));
            Assert.That(_boardEval.NonTurnMoves, Contains.Key(new BoardPosition(1, 1)));
            Assert.That(_boardEval.NonTurnMoves, Contains.Key(new BoardPosition(1, 2)));
            Assert.That(_boardEval.TurnMoves.Count(), Is.EqualTo(2));
            Assert.That(_boardEval.NonTurnMoves.Count(), Is.EqualTo(2));
        }

        [Test]
        public void OnWhiteTurn_MovesAreDividedBetweenTurn()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardEval.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardEval.TurnMoves, Contains.Key(new BoardPosition(1, 1)));
            Assert.That(_boardEval.TurnMoves, Contains.Key(new BoardPosition(1, 2)));
            Assert.That(_boardEval.NonTurnMoves, Contains.Key(new BoardPosition(7, 7)));
            Assert.That(_boardEval.NonTurnMoves, Contains.Key(new BoardPosition(7, 6)));
            Assert.That(_boardEval.TurnMoves.Count(), Is.EqualTo(2));
            Assert.That(_boardEval.NonTurnMoves.Count(), Is.EqualTo(2));
        }

        [Test]
        public void OnBlackTurn_FriendlyKingIsFound()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardEval.EvaluateBoard(boardState, PieceColour.Black);

            Assert.That(_boardEval.KingPosition, Is.EqualTo(new BoardPosition(7, 7)));
        }


        [Test]
        public void OnWhiteTurn_FriendlyKingIsFound()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardEval.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardEval.KingPosition, Is.EqualTo(new BoardPosition(1, 1)));
        }

        [Test]
        public void KingPositionNullIsOutOfBound()
        {
            var board = _boardGenerator.GenerateBoard();

            board[1, 2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            _boardEval.EvaluateBoard(boardState, PieceColour.White);

            Assert.That(_boardEval.KingPosition, Is.EqualTo(new BoardPosition(8, 8)));
        }
    }
}