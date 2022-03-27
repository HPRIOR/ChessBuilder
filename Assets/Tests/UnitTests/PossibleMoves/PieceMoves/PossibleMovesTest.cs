using System.Collections.Generic;
using System.Linq;
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

        private IMovesGenerator _movesGenerator;

        private void ResolveContainer()
        {
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
                var pieceDict = new Dictionary<Position, PieceType>()
                {
                    { new Position(1, 1), pieceType }
                };

                var boardState = new BoardState(pieceDict);
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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(1, 1), pieceType }
            };

            var boardState = new BoardState(pieceDict);
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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 1), pieceType }
            };

            var boardState = new BoardState(pieceDict);
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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 1), pieceType }
            };

            var boardState = new BoardState(pieceDict);
            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(0, moveState.PossibleMoves.SelectMany(x => x.Value).Count());
        }

        [Test]
        public void WhenCheckedAndNoInterceptAvailable_OnlyKingCanMoveToAvoid()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 6), PieceType.BlackKing },
                { new Position(4, 6), PieceType.BlackPawn },
                { new Position(1, 1), PieceType.WhiteQueen }
            };

            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(6, moveState.PossibleMoves[new Position(1, 6)].Count());
        }

        [Test]
        public void WhenChecked_BlackQueenCanIntercept()
        {
            var blackQueenPosition = new Position(4, 6);
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 6), PieceType.BlackKing },
                { new Position(4, 6), PieceType.BlackQueen },
                { new Position(1, 1), PieceType.WhiteQueen }
            };

            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(1, moveState.PossibleMoves[blackQueenPosition].Count());
            Assert.IsTrue(moveState.PossibleMoves[blackQueenPosition].Contains(new Position(1, 3)));
        }

        [Test]
        public void WhenChecked_BlackPawnCannotMove(
        )
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 6), PieceType.BlackKing },
                { new Position(4, 6), PieceType.BlackPawn },
                { new Position(1, 1), PieceType.WhiteQueen }
            };

            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);
            Assert.AreEqual(0, moveState.PossibleMoves[new Position(4, 6)].Count());
        }

        [Test]
        public void WhenCheckByMoreThanOnePiece_OnlyKingCanMove()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 6), PieceType.BlackKing },
                { new Position(4, 6), PieceType.BlackQueen },
                { new Position(1, 1), PieceType.WhiteQueen },
                { new Position(6, 1), PieceType.WhiteQueen }
            };

            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(1, 6), PieceType.BlackKing },
                { new Position(1, 1), PieceType.WhiteQueen },
                { new Position(6, 1), PieceType.WhiteQueen }
            };

            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(0, 0), PieceType.WhiteKing },
                { new Position(3, 5), PieceType.WhiteQueen },
                { new Position(7, 7), PieceType.BlackQueen }
            };


            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(0, 0), PieceType.WhiteKing },
                { new Position(3, 3), PieceType.WhiteQueen },
                { new Position(1, 1), PieceType.BlackPawn }
            };


            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(0, 0), PieceType.WhiteKing },
                { new Position(4, 2), PieceType.WhiteQueen },
                { new Position(1, 1), PieceType.BlackPawn }
            };

            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.PossibleMoves[new Position(4, 2)].Any(), Is.False);
        }

        [Test]
        public void KingCanTakeToAvoidCheck()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(0, 0), PieceType.WhiteKing },
                { new Position(1, 1), PieceType.BlackKing }
            };

            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.PossibleMoves[new Position(0, 0)].Any(), Is.True);
        }

        [Test]
        public void PawnTakingMovesAreTakenFromKingMoves()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(4, 4), PieceType.WhiteKing },
                { new Position(4, 6), PieceType.BlackPawn }
            };

            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(4, 4), PieceType.WhiteKing },
                { new Position(4, 6), PieceType.BlackPawn },
                { new Position(7, 4), PieceType.BlackRook }
            };

            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(4, 4), PieceType.WhiteKing },
                { new Position(4, 6), PieceType.BlackPawn },
                { new Position(7, 4), PieceType.BlackRook }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.Check, Is.True);
        }


        [Test]
        public void WhenNotInCheck_MoveStateIsNot()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(4, 4), PieceType.WhiteKing },
                { new Position(4, 6), PieceType.BlackPawn }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            Assert.That(moveState.Check, Is.False);
        }

        [Test]
        public void KingCannotTakeProtectedPiece()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(4, 4), PieceType.WhiteKing },
                { new Position(4, 5), PieceType.BlackPawn },
                { new Position(7, 5), PieceType.BlackRook }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleKingMoves = moveState.PossibleMoves[new Position(4, 4)];

            Assert.That(possibleKingMoves.Contains(new Position(4, 5)), Is.False);
        }

        [Test]
        public void RookCanPinPiece()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 4), PieceType.WhiteKing },
                { new Position(4, 4), PieceType.WhitePawn },
                { new Position(7, 4), PieceType.BlackRook }
            };

            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possiblePawnMoves = moveState.PossibleMoves[new Position(4, 4)];
            Assert.AreEqual(0, possiblePawnMoves.Count());
        }


        [Test]
        public void QueenCanPinPiece()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 4), PieceType.WhiteKing },
                { new Position(4, 4), PieceType.WhitePawn },
                { new Position(7, 4), PieceType.BlackQueen }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possiblePawnMoves = moveState.PossibleMoves[new Position(4, 4)];
            Assert.AreEqual(0, possiblePawnMoves.Count());
        }


        [Test]
        public void BishopCanPinPiece()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 4), PieceType.WhiteKing },
                { new Position(4, 3), PieceType.WhitePawn },
                { new Position(7, 0), PieceType.BlackBishop }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possiblePawnMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.AreEqual(0, possiblePawnMoves.Count());
        }

        [Test]
        public void WhenPinned_PieceCanTakePinningPiece()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 4), PieceType.WhiteKing },
                { new Position(4, 3), PieceType.WhiteQueen },
                { new Position(7, 0), PieceType.BlackBishop }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.That(possibleQueenMoves, Does.Contain(new Position(7, 0)));
        }

        [Test]
        public void PinIsBlockedByFriendInBetween()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 4), PieceType.WhiteKing },
                { new Position(4, 3), PieceType.WhiteQueen },
                { new Position(6, 1), PieceType.BlackBishop },
                { new Position(7, 0), PieceType.BlackBishop }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.That(possibleQueenMoves.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void PinIsBlockedByEnemyInBetween()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 4), PieceType.WhiteKing },
                { new Position(4, 3), PieceType.WhiteQueen },
                { new Position(6, 1), PieceType.WhiteBishop },
                { new Position(7, 0), PieceType.BlackBishop }
            };


            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.White);

            var possibleQueenMoves = moveState.PossibleMoves[new Position(4, 3)];
            Assert.That(possibleQueenMoves.Count(), Is.GreaterThan(0));
        }


        [Test]
        public void WhenPinned_PieceCanMoveIntoAnotherBlockingPosition()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 4), PieceType.WhiteKing },
                { new Position(4, 3), PieceType.WhiteQueen },
                { new Position(7, 0), PieceType.BlackBishop }
            };


            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(4, 1), PieceType.WhiteKing },
                { new Position(3, 2), PieceType.WhiteQueen },
                { new Position(1, 4), PieceType.BlackQueen }
            };

            var boardState = new BoardState(pieceDict);

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
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(2, 7), PieceType.BlackPawn },
                { new Position(3, 6), PieceType.BlackKing },
                { new Position(4, 5), PieceType.BlackBishop },
                { new Position(6, 3), PieceType.WhiteQueen }
            };

            var boardState = new BoardState(pieceDict);

            var moveState =
                _movesGenerator.GetPossibleMoves(boardState, PieceColour.Black);

            var possibleBishopMoves = moveState.PossibleMoves[new Position(4, 5)];
            Assert.That(possibleBishopMoves,
                Is.EquivalentTo(new List<Position>
                    { new Position(5, 4), new Position(6, 3) }));
        }
    }
}