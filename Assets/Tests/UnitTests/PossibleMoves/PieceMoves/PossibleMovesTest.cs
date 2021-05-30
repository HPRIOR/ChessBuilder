using System.Linq;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using Tests.UnitTests.PossibleMoves.PieceMoves.Utils;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    [TestFixture]
    public class PossibleMovesTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Init()
        {
            PossibleMovesBinder.InstallBindings(Container);
            ResolveContainer();
        }

        [TearDown]
        public void TearDown()
        {
            Container.UnbindAll();
        }

        private IBoardGenerator _boardGenerator;
        private IAllPossibleMovesGenerator _allPossibleMovesGenerator;

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _allPossibleMovesGenerator = Container.Resolve<IAllPossibleMovesGenerator>();
        }

        [Test]
        public void WithNoPieces_NoPossibleMoves()
        {
            var boardState = new BoardState(_boardGenerator);
            var possibleMoves =
                _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.White, new BoardPosition(0, 4));
            Assert.AreEqual(0, possibleMoves.SelectMany(x => x.Value).Count());
        }

        [Test]
        public void OnWhiteTurn_BlackCannotMove(
            [Values(PieceType.BlackBishop, PieceType.BlackKing, PieceType.BlackKnight, PieceType.BlackPawn,
                PieceType.BlackQueen, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            {
                var board = _boardGenerator.GenerateBoard();
                board[1, 1].CurrentPiece = new Piece(pieceType);
                var boardState = new BoardState(board);
                var possibleMoves =
                    _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.White, new BoardPosition(0, 4));
                Assert.AreEqual(0, possibleMoves.SelectMany(x => x.Value).Count());
            }
        }

        [Test]
        public void OnBlackTurn_BlackCanMove(
            [Values(PieceType.BlackBishop, PieceType.BlackKing, PieceType.BlackKnight, PieceType.BlackPawn,
                PieceType.BlackQueen, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(pieceType);
            var boardState = new BoardState(board);
            var possibleMoves =
                _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.Black, new BoardPosition(0, 4));
            Assert.Greater(possibleMoves.SelectMany(x => x.Value).Count(), 0);
        }

        [Test]
        public void OnWhiteTurn_WhiteCanMove(
            [Values(PieceType.WhiteKnight, PieceType.WhiteKing, PieceType.WhiteBishop, PieceType.WhiteQueen,
                PieceType.WhitePawn, PieceType.WhiteRook)]
            PieceType pieceType
        )
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(pieceType);
            var boardState = new BoardState(board);
            var possibleMoves =
                _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.White, new BoardPosition(0, 4));
            Assert.Greater(possibleMoves.SelectMany(x => x.Value).Count(), 0);
        }

        [Test]
        public void OnBlackTurn_WhiteCannotMove(
            [Values(PieceType.WhiteKnight, PieceType.WhiteKing, PieceType.WhiteBishop, PieceType.WhiteQueen,
                PieceType.WhitePawn, PieceType.WhiteRook)]
            PieceType pieceType
        )
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 1].CurrentPiece = new Piece(pieceType);
            var boardState = new BoardState(board);
            var possibleMoves =
                _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.Black, new BoardPosition(0, 4));
            Assert.AreEqual(0, possibleMoves.SelectMany(x => x.Value).Count());
        }

        [Test]
        public void WhenChecked_OnlyKingCanMoveToAvoid()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);
            var boardState = new BoardState(board);

            var possibleMoves =
                _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.Black, new BoardPosition(1, 1));
            Assert.AreEqual(6, possibleMoves[new BoardPosition(1, 6)].Count());
        }

        [Test]
        public void WhenChecked_BlackQueenCanIntercept()
        {
            var blackQueenPosition = new BoardPosition(4, 6);
            var board = _boardGenerator.GenerateBoard();
            board[1, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackQueen);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);
            var boardState = new BoardState(board);

            var possibleMoves =
                _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.Black, new BoardPosition(1, 1));
            Assert.AreEqual(1, possibleMoves[blackQueenPosition].Count());
            Assert.IsTrue(possibleMoves[blackQueenPosition].Contains(new BoardPosition(1, 3)));
        }

        [Test]
        public void WhenChecked_BlackPawnCannotMove(
        )
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);
            var boardState = new BoardState(board);

            var possibleMoves =
                _allPossibleMovesGenerator.GetPossibleMoves(boardState, PieceColour.Black, new BoardPosition(1, 1));
            Assert.AreEqual(0, possibleMoves[new BoardPosition(4, 6)].Count());
        }
    }
}