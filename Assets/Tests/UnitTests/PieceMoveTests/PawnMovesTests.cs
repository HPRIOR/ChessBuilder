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
    public void PawnsCanMoveForward(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y, 
        [Values(PieceType.WhitePawn, PieceType.BlackPawn)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);

        var pawnMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType, GetRelativePosition(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(1, possibleMoves.Count());
        Assert.AreEqual(GetRelativePosition(new BoardPosition(x, y + 1)), possibleMoves.First());
    }

    [Test]
    public void PawnsCanTakeDiagonallyRight(
        [Values(0, 1, 2, 3, 4, 5, 6)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.WhitePawn, PieceType.BlackPawn)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var pawnMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType, GetRelativePosition(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), GetRelativePosition(new BoardPosition(x + 1, y + 1)))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(2, possibleMoves.Count());
        Assert.Contains(GetRelativePosition(new BoardPosition(x + 1, y + 1)), (ICollection)possibleMoves);

    }

    [Test]
    public void PawnsCanTakeDiagonallyLeft(
        [Values(1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.WhitePawn, PieceType.BlackPawn)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var pawnMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType,  GetRelativePosition(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType),  GetRelativePosition(new BoardPosition(x - 1, y + 1)))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(2, possibleMoves.Count());
        Assert.Contains(GetRelativePosition(new BoardPosition(x - 1, y + 1)), (ICollection)possibleMoves);
    }

    [Test]
    public void PawnIsBlockedByWhiteAndBlackPieces(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.BlackPawn, PieceType.WhitePawn)] PieceType whiteOrBlackPiece,
        [Values(PieceType.BlackPawn, PieceType.WhitePawn)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var pawnMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType, GetRelativePosition(new BoardPosition(x, y))),
            (whiteOrBlackPiece, GetRelativePosition(new BoardPosition(x, y + 1)))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(0, possibleMoves.Count());
    }

    [Test]
    public void PawnsCanTakeOnBothSide(
        [Values(1, 2, 3, 4, 5, 6)] int x, [Values(0, 1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.BlackPawn, PieceType.WhitePawn)] PieceType pieceType
        )
    {

        SetTestedPieceColourWith(pieceType);
        var pawnMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>() {
            (pieceType, GetRelativePosition(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), GetRelativePosition(new BoardPosition(x + 1, y + 1))),
            (GetOppositePieceType(pieceType), GetRelativePosition(new BoardPosition(x - 1, y + 1)))
        };

        SetUpBoardWith(pieces);

        var pawnGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        Assert.AreEqual(3, possibleMoves.Count());
        Assert.Contains(GetRelativePosition(new BoardPosition(x - 1, y + 1)), (ICollection)possibleMoves);
        Assert.Contains(GetRelativePosition(new BoardPosition(x + 1, y + 1)), (ICollection)possibleMoves);
    }

}