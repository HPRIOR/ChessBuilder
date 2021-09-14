using System.Collections.Generic;
using System.Linq;
using Models.Services.Board;
using Models.Services.Moves.Interfaces;
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
        private IMovesGenerator _movesGenerator;

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _movesGenerator = Container.Resolve<IMovesGenerator>();
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
                var moveState =
                    _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);
                Assert.AreEqual(0, moveState.PossibleMoves.SelectMany(x => x.Value).Count());
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
            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.Greater(moveState.PossibleMoves.SelectMany(x => x.Value).Count(), 0);
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
            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);
            Assert.Greater(moveState.PossibleMoves.SelectMany(x => x.Value).Count(), 0);
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
            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(0, moveState.PossibleMoves.SelectMany(x => x.Value).Count());
        }

        [Test]
        public void WhenCheckedAndNoInterceptAvailable_OnlyKingCanMoveToAvoid()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);

            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(6, moveState.PossibleMoves[new Position(1, 6)].Count());
        }

        [Test]
        public void WhenChecked_BlackQueenCanIntercept()
        {
            var blackQueenPosition = new Position(4, 6);
            var board = _boardGenerator.GenerateBoard();
            board[1, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackQueen);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);

            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(1, moveState.PossibleMoves[blackQueenPosition].Count());
            Assert.IsTrue(moveState.PossibleMoves[blackQueenPosition].Contains(new Position(1, 3)));
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

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(0, moveState.PossibleMoves[new Position(4, 6)].Count());
        }

        [Test]
        public void WhenCheckByMoreThanOnePiece_OnlyKingCanMove()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackQueen);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[6, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);

            var activeBuilds = new HashSet<Position>();
            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);

            // no moves for black queen 
            Assert.AreEqual(0, moveState.PossibleMoves[new Position(4, 6)].Count());

            // moves for black king 
            Assert.IsTrue(moveState.PossibleMoves[new Position(1, 6)].Any());
        }

        [Test]
        public void WhenCheckByMoreThanOnePiece_KingsMovesAreReducesByAll()
        {
            var board = _boardGenerator.GenerateBoard();
            board[1, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[1, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[6, 1].CurrentPiece = new Piece(PieceType.WhiteQueen);

            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);

            var expectedMoves = new HashSet<Position>
            {
                new Position(0, 6),
                new Position(0, 5),
                new Position(2, 6),
                new Position(2, 7)
            };
            var kingMoves = moveState.PossibleMoves[new Position(1, 6)];
            Assert.That(kingMoves, Is.EquivalentTo(expectedMoves));
        }

        [Test]
        public void WhenInCheck_OnlyInterceptingMovesAreGiven()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[3, 5].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[7, 7].CurrentPiece = new Piece(PieceType.BlackQueen);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var expected = new HashSet<Position>
            {
                new Position(3, 3),
                new Position(4, 4),
                new Position(5, 5)
            };
            Assert.That(moveState.PossibleMoves[new Position(3, 5)], Is.EquivalentTo(expected));
        }

        [Test]
        public void WhenInCheck_InterceptingMovesIncludeCheckingPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[3, 3].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var expected = new HashSet<Position>
            {
                new Position(1, 1)
            };
            Assert.That(moveState.PossibleMoves[new Position(3, 3)], Is.EquivalentTo(expected));
        }

        [Test]
        public void PawnCanCheckKing()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 2].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackPawn);

            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.PossibleMoves[new Position(4, 2)].Any(), Is.False);
        }

        [Test]
        public void KingCanTakeToAvoidCheck()
        {
            var board = _boardGenerator.GenerateBoard();
            board[0, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[1, 1].CurrentPiece = new Piece(PieceType.BlackKing);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.PossibleMoves[new Position(0, 0)].Any(), Is.True);
        }

        [Test]
        public void PawnTakingMovesAreTakenFromKingMoves()
        {
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleKingMoves = moveState.PossibleMoves[new Position(4, 4)];

            Assert.That(possibleKingMoves.Contains(new Position(4, 5)), Is.True);
            Assert.That(possibleKingMoves.Contains(new Position(3, 5)), Is.False);
            Assert.That(possibleKingMoves.Contains(new Position(5, 5)), Is.False);
        }

        [Test]
        public void WhenInCheck_PawnTakingMovesAreTakenFromKingMoves()
        {
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[7, 4].CurrentPiece = new Piece(PieceType.BlackRook);

            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleKingMoves = moveState.PossibleMoves[new Position(4, 4)];

            Assert.That(possibleKingMoves.Contains(new Position(4, 5)), Is.True);
            Assert.That(possibleKingMoves.Contains(new Position(3, 5)), Is.False);
            Assert.That(possibleKingMoves.Contains(new Position(5, 5)), Is.False);
        }


        [Test]
        public void WhenInCheck_MoveStateIsCheck()
        {
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[7, 4].CurrentPiece = new Piece(PieceType.BlackRook);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.Check, Is.True);
        }


        [Test]
        public void WhenNotInCheck_MoveStateIsNot()
        {
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.Check, Is.False);
        }

        [Test]
        public void KingCannotTakeProtectedPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[4, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 5].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[7, 5].CurrentPiece = new Piece(PieceType.BlackRook);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleKingMoves = moveState.PossibleMoves[new Position(4, 4)];

            Assert.That(possibleKingMoves.Contains(new Position(4, 5)), Is.False);
        }

        [Test]
        public void RookCanPinPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 4].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 4].CurrentPiece = new Piece(PieceType.BlackRook);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possiblePawnMoves = moveState.PossibleMoves[new Position(4, 4)];
            Assert.AreEqual(0, possiblePawnMoves.Count());
        }


        [Test]
        public void QueenCanPinPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 4].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 4].CurrentPiece = new Piece(PieceType.BlackQueen);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possiblePawnMoves = moveState.PossibleMoves[new Position(4, 4)];
            Assert.AreEqual(0, possiblePawnMoves.Count());
        }


        [Test]
        public void BishopCanPinPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 3].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[7, 0].CurrentPiece = new Piece(PieceType.BlackBishop);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possiblePawnMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.AreEqual(0, possiblePawnMoves.Count());
        }

        [Test]
        public void WhenPinned_PieceCanTakePinningPiece()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 3].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[7, 0].CurrentPiece = new Piece(PieceType.BlackBishop);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.That(possibleQueenMoves, Does.Contain(new Position(7, 0)));
        }

        [Test]
        public void PinIsBlockedByFriendInBetween()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 3].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[6, 1].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[7, 0].CurrentPiece = new Piece(PieceType.BlackBishop);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.That(possibleQueenMoves.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void PinIsBlockedByEnemyInBetween()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 3].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[6, 1].CurrentPiece = new Piece(PieceType.WhiteBishop);
            board[7, 0].CurrentPiece = new Piece(PieceType.BlackBishop);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.That(possibleQueenMoves.Count(), Is.GreaterThan(0));
        }


        [Test]
        public void WhenPinned_PieceCanMoveIntoAnotherBlockingPosition()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 4].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4, 3].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[7, 0].CurrentPiece = new Piece(PieceType.BlackBishop);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.That(possibleQueenMoves,
                Is.EquivalentTo(new List<Position>
                    { new Position(7, 0), new Position(5, 2), new Position(6, 1) }));
        }

        [Test]
        public void QueenPinnedByEnemyQueen()
        {
            var board = _boardGenerator.GenerateBoard();
            board[4, 1].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[3, 2].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[1, 4].CurrentPiece = new Piece(PieceType.BlackQueen);


            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(3, 2)];
            Assert.That(possibleQueenMoves,
                Is.EquivalentTo(new List<Position>
                    { new Position(2, 3), new Position(1, 4) }));
        }


        [Test]
        public void WithPieceBehindKingOfPinnedPiece_PieceIsPinned()
        {
            var board = _boardGenerator.GenerateBoard();
            board[2, 7].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[3, 6].CurrentPiece = new Piece(PieceType.BlackKing);
            board[4, 5].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[6, 3].CurrentPiece = new Piece(PieceType.WhiteQueen);

            var boardState = new BoardState(board);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);

            var possibleBishopMoves = moveState.PossibleMoves[new Position(4, 5)];
            Assert.That(possibleBishopMoves,
                Is.EquivalentTo(new List<Position>
                    { new Position(5, 4), new Position(6, 3) }));
        }
    }
}