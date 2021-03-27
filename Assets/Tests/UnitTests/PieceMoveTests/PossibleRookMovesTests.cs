using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[TestFixture]
public class PossibleRookMovesTests : PossibleMovesTestBase
{
    [Test]
    public void OnEmptyBoard_RookCanMoveForwardAndSideWays(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        var rookGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

        Assert.AreEqual(14, new HashSet<IBoardPosition>(possibleMoves).Count());
    }

    [Test]
    public void WithOpposingPieceOnBackRankAndFile_RookCanTake(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 7))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(7, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            Assert.AreEqual(14, new HashSet<IBoardPosition>(possibleMoves).Count());
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }

    [Test]
    public void WithOpposingPiecesOnSeventhRankAndFile_RookCanTakeAndIsBlocked(
        [Values(0, 1, 2, 3, 4, 5)] int x, [Values(0, 1, 2, 3, 4, 5)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 6))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(6, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            IList<IBoardPosition> unreachableTiles = new List<IBoardPosition>()
            {
                new BoardPosition(x, 7), new BoardPosition(y, 7)
            }.Select(RelativePositionToTestedPiece).ToList();

            HashSet<IBoardPosition> reachableTiles = new HashSet<IBoardPosition>(possibleMoves);

            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }

    [Test]
    public void WithOpposingPiecesOnMidRankAndFile_RookCanTakeAndIsBlocked(
        [Values(0, 1, 2, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 4, 5, 6, 7)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 3))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(3, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);


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

            HashSet<IBoardPosition> reachableTiles = new HashSet<IBoardPosition>(possibleMoves.Select(RelativePositionToTestedPiece));

            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }


    [Test]
    public void WithFriendlyPieceOnBackRankAndFile_RookIsBlocked(
        [Values(0, 1, 2, 3, 4, 5, 6)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, 7))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(7, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            var reachableTile = new HashSet<IBoardPosition>(possibleMoves);
            var unreachableTiles =
                new HashSet<IBoardPosition>(
                    new List<IBoardPosition>() { new BoardPosition(7, y), new BoardPosition(x, 7) }
                    .Select(RelativePositionToTestedPiece));

            Assert.IsFalse(reachableTile.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }

    [Test]
    public void WithFriendlyPieceOnSeventhRankAndFile_RookIsBlocked(
        [Values(0, 1, 2, 3, 4, 5)] int x, [Values(0, 1, 2, 3, 4, 5)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, 6))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(6, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            var reachableTile = new HashSet<IBoardPosition>(possibleMoves);
            var unreachableTilesNorth =
                GetPositionsIncludingAndPassed(new BoardPosition(x, 6), Direction.N).ToList();
            var unreachableTilesEast =
                GetPositionsIncludingAndPassed(new BoardPosition(6, y), Direction.E).ToList();
            var unreachableTiles = unreachableTilesEast.Concat(unreachableTilesNorth).Select(RelativePositionToTestedPiece);

            Assert.IsFalse(reachableTile.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }

    [Test]
    public void WithFriendlyPiecesOnMidRankAndFile_RookIsBlocked(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, 3))),
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(3, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            var unreachableTilesNorth = GetPositionsIncludingAndPassed(new BoardPosition(x, 3), Direction.N).ToList();
            var unreachableTilesEast = GetPositionsIncludingAndPassed(new BoardPosition(3, y), Direction.E).ToList();

            var unreachableTilesSouth = GetPositionsIncludingAndPassed(new BoardPosition(x, 3), Direction.S).ToList();
            var unreachabletilesWest = GetPositionsIncludingAndPassed(new BoardPosition(3, y), Direction.W).ToList();


            var unreachableTiles =
                x < 3
                ? unreachableTilesEast
                : unreachabletilesWest
                .Concat(
                    y < 3
                    ? unreachableTilesNorth
                    : unreachabletilesWest);


            HashSet<IBoardPosition> reachableTiles = new HashSet<IBoardPosition>(possibleMoves);

            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }


}