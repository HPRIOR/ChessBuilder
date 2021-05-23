using System.Collections.Generic;
using System.Linq;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.PieceState;
using NUnit.Framework;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    public class PossibleKnightMovesTests : PossibleMovesTestBase
    {
        [Test]
        public void OnEmptyBoard_KnightCanMove(
            [Values(2, 3, 4, 5)] int x, [Values(2, 3, 4, 5)] int y,
            [Values(PieceType.WhiteKnight, PieceType.BlackKnight)]
            PieceType pieceType)
        {
            SetTestedPieceColourWith(pieceType);
            var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
            };

            var board = SetUpBoardWith(pieces);


            var possibleMoves =
                new HashSet<IBoardPosition>(knightMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board));

            Assert.AreEqual(8, possibleMoves.Count);
        }

        [Test]
        public void OnEmptyBoard_KnightMovesToCorrectPosition(
            [Values(2, 3, 4, 5)] int x, [Values(2, 3, 4, 5)] int y,
            [Values(PieceType.WhiteKnight, PieceType.BlackKnight)]
            PieceType pieceType)
        {
            SetTestedPieceColourWith(pieceType);
            var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
            };

            var board = SetUpBoardWith(pieces);


            var expectedMoves = new List<IBoardPosition>
            {
                new BoardPosition(x + 2, y + 1),
                new BoardPosition(x + 2, y - 1),
                new BoardPosition(x - 2, y + 1),
                new BoardPosition(x - 2, y - 1),
                new BoardPosition(x + 1, y + 2),
                new BoardPosition(x - 1, y + 2),
                new BoardPosition(x + 1, y - 2),
                new BoardPosition(x - 1, y - 2)
            };
            var possibleMoves =
                new HashSet<IBoardPosition>(knightMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board));
            expectedMoves.ForEach(move => Assert.IsTrue(possibleMoves.Contains(move)));
        }

        [Test]
        public void WithFriendlyPiece_KnightIsBlocked(
            [Values(2, 3, 4, 5)] int x, [Values(2, 3, 4, 5)] int y,
            [Values(PieceType.WhiteKnight, PieceType.BlackKnight)]
            PieceType pieceType)
        {
            SetTestedPieceColourWith(pieceType);
            var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x + 2, y + 1)))
            };

            var board = SetUpBoardWith(pieces);

            var expectedMoves = new List<IBoardPosition>
            {
                new BoardPosition(x + 2, y - 1),
                new BoardPosition(x - 2, y + 1),
                new BoardPosition(x - 2, y - 1),
                new BoardPosition(x + 1, y + 2),
                new BoardPosition(x - 1, y + 2),
                new BoardPosition(x + 1, y - 2),
                new BoardPosition(x - 1, y - 2)
            }.Select(RelativePositionToTestedPiece);

            var illegalMove = RelativePositionToTestedPiece(new BoardPosition(x + 2, y + 1));
            var possibleMoves = new HashSet<IBoardPosition>(
                knightMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)),
                    board));

            expectedMoves.ToList().ForEach(move => Assert.IsTrue(possibleMoves.Contains(move)));
            Assert.AreEqual(7, expectedMoves.Count());
            Assert.IsFalse(possibleMoves.Contains(illegalMove));
        }

        [Test]
        public void WithOpposingPiece_KnightCanTake(
            [Values(2, 3, 4, 5)] int x, [Values(2, 3, 4, 5)] int y,
            [Values(PieceType.WhiteKnight, PieceType.BlackKnight)]
            PieceType pieceType)
        {
            SetTestedPieceColourWith(pieceType);
            var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x + 2, y + 1)))
            };

            var board = SetUpBoardWith(pieces);

            var expectedMoves = new List<IBoardPosition>
            {
                new BoardPosition(x + 2, y + 1),
                new BoardPosition(x + 2, y - 1),
                new BoardPosition(x - 2, y + 1),
                new BoardPosition(x - 2, y - 1),
                new BoardPosition(x + 1, y + 2),
                new BoardPosition(x - 1, y + 2),
                new BoardPosition(x + 1, y - 2),
                new BoardPosition(x - 1, y - 2)
            }.Select(RelativePositionToTestedPiece);

            var possibleMoves = new HashSet<IBoardPosition>(
                knightMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)),
                    board));

            expectedMoves.ToList().ForEach(move => Assert.IsTrue(possibleMoves.Contains(move)));
            Assert.AreEqual(8, expectedMoves.Count());
        }

        [Test]
        public void WithPieceInCorner_OnlyTwoMovesArePossible(
            [Values(0, 7)] int x, [Values(0, 7)] int y,
            [Values(PieceType.WhiteKnight, PieceType.BlackKnight)]
            PieceType pieceType)
        {
            SetTestedPieceColourWith(pieceType);
            var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
            };

            var board = SetUpBoardWith(pieces);
            var possibleMoves = knightMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);
            Assert.AreEqual(2, possibleMoves.Count());
        }

        [Test]
        public void OnSide_OnlyFourMovesArePossible(
            [Values(2, 3, 4, 5)] int x,
            [Values(PieceType.WhiteKnight, PieceType.BlackKnight)]
            PieceType pieceType)
        {
            SetTestedPieceColourWith(pieceType);
            var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, 0)))
            };

            var board = SetUpBoardWith(pieces);
            var possibleMoves = knightMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, 0), board);
            Assert.AreEqual(4, possibleMoves.Count());
        }
    }
}