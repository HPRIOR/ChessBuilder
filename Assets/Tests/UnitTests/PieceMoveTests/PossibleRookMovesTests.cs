using Zenject;
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
            (pieceType, GetRelativePosition(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        var rookGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

        Assert.AreEqual(14, possibleMoves.Count());
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
            (pieceType, GetRelativePosition(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), GetRelativePosition(new BoardPosition(x, 7))),
            (GetOppositePieceType(pieceType), GetRelativePosition(new BoardPosition(7, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            Assert.AreEqual(14, possibleMoves.Count());
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }

    [Test]
    public void WithOpposingPiecesOnSeventhRankAndFile_RookCanTakeAndIsBlocked(
        [Values(0, 1, 2, 3, 4, 5, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 7)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType, GetRelativePosition(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), GetRelativePosition(new BoardPosition(x, 6))),
            (GetOppositePieceType(pieceType), GetRelativePosition(new BoardPosition(6, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            IList<IBoardPosition> unreachableTiles = new List<IBoardPosition>()
            {
                new BoardPosition(x, 7), new BoardPosition(y, 7)
            }.Select(GetRelativePosition).ToList();

            HashSet<IBoardPosition> reachableTiles = new HashSet<IBoardPosition>(possibleMoves);

            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }

    [Test]
    public void WithFriendlyPieceOnBackRankAndFile_RookIsBlocked(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var rookMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType, GetRelativePosition(new BoardPosition(x, y))),
            (pieceType, GetRelativePosition(new BoardPosition(x, 7))),
            (pieceType, GetRelativePosition(new BoardPosition(7, y))),
            };

            SetUpBoardWith(pieces);

            var rookGameObject = GetGameObjectAtPosition(x, y);
            var possibleMoves = rookMoveGenerator.GetPossiblePieceMoves(rookGameObject);

            var reachableTile = new HashSet<IBoardPosition>(possibleMoves);
            var unreachableTiles = 
                new HashSet<IBoardPosition>(
                    new List<IBoardPosition>(){ new BoardPosition(7, y), new BoardPosition(x, 7)}
                    .Select(GetRelativePosition));

            Assert.IsFalse(reachableTile.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn clash");
        }
    }

}