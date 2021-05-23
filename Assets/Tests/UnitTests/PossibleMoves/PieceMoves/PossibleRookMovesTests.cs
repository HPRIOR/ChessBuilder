using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;

namespace Tests.UnitTests.PossibleMoves.PieceMoves
{
    [TestFixture]
    public class PossibleRookMovesTests : PossibleMovesTestBase
    {
        [Test]
        public void OnEmptyBoard_RookCanMoveForwardAndSideWays(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
            [Values(PieceType.WhiteRook, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                rookMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

            Assert.AreEqual(14, new HashSet<BoardPosition>(possibleMoves).Count());
        }

        [Test]
        public void WithOpposingPieceOnBackRankAndFile_RookCanTake(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
            [Values(PieceType.WhiteRook, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 7))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(7, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                rookMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

            Assert.AreEqual(14, new HashSet<BoardPosition>(possibleMoves).Count());
        }

        [Test]
        public void WithOpposingPiecesOnSeventhRankAndFile_RookCanTakeAndIsBlocked(
            [Values(0, 1, 2, 3, 4, 5)] int x, [Values(0, 1, 2, 3, 4, 5)] int y,
            [Values(PieceType.WhiteRook, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 6))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(6, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                rookMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

            IList<BoardPosition> unreachableTiles = new List<BoardPosition>
            {
                new BoardPosition(x, 7), new BoardPosition(y, 7)
            }.Select(RelativePositionToTestedPiece).ToList();

            var reachableTiles = new HashSet<BoardPosition>(possibleMoves);

            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }

        [Test]
        public void WithOpposingPiecesOnMidRankAndFile_RookCanTakeAndIsBlocked(
            [Values(0, 1, 2, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 4, 5, 6, 7)] int y,
            [Values(PieceType.WhiteRook, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 3))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(3, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                rookMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);


            var unreachableTilesNorth = GetPositionsIncludingAndPassed(new BoardPosition(x, 4), Direction.N).ToList();
            var unreachableTilesEast = GetPositionsIncludingAndPassed(new BoardPosition(4, y), Direction.E).ToList();

            var unreachableTilesSouth = GetPositionsIncludingAndPassed(new BoardPosition(x, 2), Direction.S).ToList();
            var unreachabletilesWest = GetPositionsIncludingAndPassed(new BoardPosition(2, y), Direction.W).ToList();


            var unreachableTiles =
                x < 3
                    ? unreachableTilesEast
                    : unreachabletilesWest
                        .Concat(
                            y < 3
                                ? unreachableTilesNorth
                                : unreachabletilesWest);

            var reachableTiles = new HashSet<BoardPosition>(possibleMoves.Select(RelativePositionToTestedPiece));

            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }


        [Test]
        public void WithFriendlyPieceOnBackRankAndFile_RookIsBlocked(
            [Values(0, 1, 2, 3, 4, 5, 6)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y,
            [Values(PieceType.WhiteRook, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, 7))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(7, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                rookMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

            var reachableTile = new HashSet<BoardPosition>(possibleMoves);
            var unreachableTiles =
                new HashSet<BoardPosition>(
                    new List<BoardPosition> {new BoardPosition(7, y), new BoardPosition(x, 7)}
                        .Select(RelativePositionToTestedPiece));

            Assert.IsFalse(reachableTile.Overlaps(unreachableTiles));
        }

        [Test]
        public void WithFriendlyPieceOnSeventhRankAndFile_RookIsBlocked(
            [Values(0, 1, 2, 3, 4, 5)] int x, [Values(0, 1, 2, 3, 4, 5)] int y,
            [Values(PieceType.WhiteRook, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, 6))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(6, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                rookMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

            var reachableTile = new HashSet<BoardPosition>(possibleMoves);
            var unreachableTilesNorth =
                GetPositionsIncludingAndPassed(new BoardPosition(x, 6), Direction.N).ToList();
            var unreachableTilesEast =
                GetPositionsIncludingAndPassed(new BoardPosition(6, y), Direction.E).ToList();
            var unreachableTiles =
                unreachableTilesEast.Concat(unreachableTilesNorth).Select(RelativePositionToTestedPiece);

            Assert.IsFalse(reachableTile.Overlaps(unreachableTiles));
        }

        [Test]
        public void WithFriendlyPiecesOnMidRankAndFile_RookIsBlocked(
            [Values(0, 1, 2, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 4, 5, 6, 7)] int y,
            [Values(PieceType.WhiteRook, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(PieceType.WhiteRook);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, BoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, 3))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(3, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                rookMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

            var unreachableTilesNorth =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(x, 3)), Direction.N)
                    .ToList();
            var unreachableTilesEast =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(3, y)), Direction.E)
                    .ToList();

            var unreachableTilesSouth =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(x, 3)), Direction.S)
                    .ToList();
            var unreachabletilesWest =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(3, y)), Direction.W)
                    .ToList();


            var unreachableTiles =
                x < 3
                    ? unreachableTilesEast
                    : unreachabletilesWest
                        .Concat(
                            y < 3
                                ? unreachableTilesNorth
                                : unreachabletilesWest);

            var reachableTiles = new HashSet<BoardPosition>(possibleMoves);

            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }
    }
}