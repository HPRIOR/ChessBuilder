using Zenject;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[TestFixture]
public class PawnMovesTests : PossibleMoveTestBase
{
    [Test]
    public void WhitePawnCanMoveUp(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y
        )
    {
        var pawnMoveGenerator = GetPossibleMoveGenerator(PieceType.WhitePawn);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (PieceType.WhitePawn, new BoardPosition(x, y))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);

        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(1, possibleMoves.Count());
        Assert.AreEqual(new BoardPosition(x, y + 1), possibleMoves.First());
    }

    [Test]
    public void WhitePawnCanTakeTopRight(
        [Values(0, 1, 2, 3, 4, 5, 6)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y
        )
    {
        var pawnMoveGenerator = GetPossibleMoveGenerator(PieceType.WhitePawn);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (PieceType.WhitePawn, new BoardPosition(x,y)),
            (PieceType.BlackPawn, new BoardPosition(x + 1, y + 1))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(2, possibleMoves.Count());
        Assert.Contains(new BoardPosition(x + 1, y + 1), (ICollection)possibleMoves);

    }

    [Test]
    public void WhitePawnCanTakeTopLeft(
        [Values(1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y
        )
    {
        var pawnMoveGenerator = GetPossibleMoveGenerator(PieceType.WhitePawn);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (PieceType.WhitePawn, new BoardPosition(x, y)),
            (PieceType.BlackPawn, new BoardPosition(x - 1, y + 1))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(2, possibleMoves.Count());
        Assert.Contains(new BoardPosition(x - 1, y + 1), (ICollection)possibleMoves);
    }

    [Test]
    public void WhitePawnIsBlockedByWhiteAndBlackPieces(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y, [Values(PieceType.BlackPawn, PieceType.WhitePawn)] PieceType whiteOrBlackPiece
        )
    {
        var pawnMoveGenerator = GetPossibleMoveGenerator(PieceType.WhitePawn);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (PieceType.WhitePawn, new BoardPosition(x, y)),
            (whiteOrBlackPiece, new BoardPosition(x, y + 1))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(0, possibleMoves.Count());
    }

    [Test]
    public void WhitePawnCanTakeBothSides(
        [Values(1, 2, 3, 4, 5, 6)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y
        )
    {
        var pawnMoveGenerator = GetPossibleMoveGenerator(PieceType.WhitePawn);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (PieceType.WhitePawn, new BoardPosition(x, y)),
            (PieceType.BlackPawn, new BoardPosition(x + 1, y + 1)),
            (PieceType.BlackPawn, new BoardPosition(x - 1, y + 1)),
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(3, possibleMoves.Count());
        Assert.Contains(new BoardPosition(x - 1, y + 1), (ICollection)possibleMoves);
        Assert.Contains(new BoardPosition(x + 1, y + 1), (ICollection)possibleMoves);
    }

    [Test]
    public void BlackPawnCanMoveDown(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        var pawnMoveGenerator = GetPossibleMoveGenerator(PieceType.BlackPawn);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (PieceType.BlackPawn, new BoardPosition(x, y))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);

        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(1, possibleMoves.Count());
        Assert.AreEqual(new BoardPosition(x, y - 1), possibleMoves.First());
    }

    [Test]
    public void BlackPawnCanTakeBottomRight(
        [Values(0, 1, 2, 3, 4, 5, 6)] int x, [Values(1, 2, 3, 4, 5, 6, 7)] int y
        )
    {
        var pawnMoveGenerator = GetPossibleMoveGenerator(PieceType.BlackPawn);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (PieceType.BlackPawn, new BoardPosition(x, y)),
            (PieceType.WhitePawn, new BoardPosition(x + 1, y - 1))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(2, possibleMoves.Count());
        Assert.Contains(new BoardPosition(x + 1, y - 1), (ICollection)possibleMoves);

    }

}