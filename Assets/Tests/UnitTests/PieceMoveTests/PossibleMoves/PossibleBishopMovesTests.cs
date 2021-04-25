using Zenject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.Piece;
using UnityEngine;

[TestFixture]
public class PossibleBishopMovesTests : PossibleMovesTestBase
{
    [Test]
    public void OnEmptyBoard_BishopCanMoveDiagonally(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteBishop, PieceType.BlackBishop)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var bishopMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y))
        };

        var board = SetUpBoardWith(pieces);
        var possibleMoves = bishopMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);
        Assert.AreEqual(7, new HashSet<IBoardPosition>(possibleMoves).Count());
    }

    [Test]
    public void WithOpposingPieceInCorner_BishopCanTake(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteBishop, PieceType.BlackBishop)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var bishopMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (GetOppositePieceType(pieceType), new BoardPosition(x == 7 ? 0 : 7, y == 7 ? 0 : 7))
        };

        var board = SetUpBoardWith(pieces);
        var possibleMoves = bishopMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);

        Assert.AreEqual(7, new HashSet<IBoardPosition>(possibleMoves).Count());

    }

    [Test]
    public void WithOpposingPieceInMid_BishopCanTakeAndIsBlocked(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteBishop, PieceType.BlackBishop)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var bishopMoveGenerator = GetPossibleMoveGenerator(pieceType);

        bool onWhiteBand = x == 7 && y == 7 || x == 0 && y == 0;
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (GetOppositePieceType(pieceType), 
                onWhiteBand ? new BoardPosition(3,3) : new BoardPosition(4, 3)
            )
        };

        var board = SetUpBoardWith(pieces);


        var unreachablePositionsNE = GetPositionsIncludingAndPassed(new BoardPosition(4, 4), Direction.NE);
        var unreachablePositionsSW = GetPositionsIncludingAndPassed(new BoardPosition(2, 2), Direction.SW);
        var unreachablePositionsNW = GetPositionsIncludingAndPassed(new BoardPosition(3, 4), Direction.NW);
        var unreachablePositionsSE = GetPositionsIncludingAndPassed(new BoardPosition(5, 2), Direction.SE);

        IList<IBoardPosition> unreachableTiles = new List<IBoardPosition>();

        if (onWhiteBand)
            if (x >= 4)
                unreachableTiles = unreachableTiles.Concat(unreachablePositionsSW).ToList();
            else
                unreachableTiles = unreachableTiles.Concat(unreachablePositionsNE).ToList();
        else
            if (x <= 3)
            unreachableTiles = unreachableTiles.Concat(unreachablePositionsSE).ToList();
        else
            unreachableTiles = unreachableTiles.Concat(unreachablePositionsNW).ToList();

        var possibleMoves = new HashSet<IBoardPosition>(bishopMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board));

        Assert.IsFalse(possibleMoves.Overlaps(unreachableTiles));

    }

    [Test]
    public void WithOpposingPieceBlockingEnd_BishopCanTakeaAndIsBlocked(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteBishop, PieceType.BlackBishop)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var bishopMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (GetOppositePieceType(pieceType), new BoardPosition(x == 0 ? 6 : 1, y == 0 ? 6 : 1))
        };

        var board = SetUpBoardWith(pieces);
        var possibleMoves = bishopMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);

        var unreachableTiles = new List<IBoardPosition>()
        {
            new BoardPosition(0,0),
            new BoardPosition(7,7),
            new BoardPosition(7,0),
            new BoardPosition(0,7),
        };

        Assert.IsFalse(new HashSet<IBoardPosition>(possibleMoves).Overlaps(unreachableTiles));
    }

    [Test]
    public void WithFriendlyPieceInCorner_BishopIsBlocked(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteBishop, PieceType.BlackBishop)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var bishopMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (pieceType, new BoardPosition(x == 7 ? 0: 7, y == 7 ? 0 : 7))
        };

        var board = SetUpBoardWith(pieces);
        var possibleMoves = bishopMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);


        Assert.AreEqual(6, new HashSet<IBoardPosition>(possibleMoves).Count());

    }

    [Test]
    public void WithFriendlyPieceBlockingEnd_BishopIsBlocked(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteBishop, PieceType.BlackBishop)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var bishopMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (pieceType, new BoardPosition(x == 0 ? 6 : 1, y == 0 ? 6 : 1))
        };

         var board = SetUpBoardWith(pieces);
        var possibleMoves = bishopMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);

        Assert.AreEqual(5, new HashSet<IBoardPosition>(possibleMoves).Count());

    }

    [Test]
    public void WithFriendlyPieceInMiddle_BishopIsBlocked(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteBishop, PieceType.BlackBishop)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var bishopMoveGenerator = GetPossibleMoveGenerator(pieceType);

        bool onWhiteBand = x == 7 && y == 7 || x == 0 && y == 0;
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (pieceType, 
                onWhiteBand ? new BoardPosition(3,3) : new BoardPosition(4, 3)
            )
        };

        var board = SetUpBoardWith(pieces);

        var unreachablePositionsNE = GetPositionsIncludingAndPassed(new BoardPosition(3, 3), Direction.NE);
        var unreachablePositionsSW = GetPositionsIncludingAndPassed(new BoardPosition(3, 3), Direction.SW);
        var unreachablePositionsNW = GetPositionsIncludingAndPassed(new BoardPosition(4, 3), Direction.NW);
        var unreachablePositionsSE = GetPositionsIncludingAndPassed(new BoardPosition(4, 3), Direction.SE);

        IList<IBoardPosition> unreachableTiles = new List<IBoardPosition>();

        if (onWhiteBand)
            if (x >= 4)
                unreachableTiles = unreachableTiles.Concat(unreachablePositionsSW).ToList();
            else
                unreachableTiles = unreachableTiles.Concat(unreachablePositionsNE).ToList();
        else
            if (x <= 3)
            unreachableTiles = unreachableTiles.Concat(unreachablePositionsSE).ToList();
        else
            unreachableTiles = unreachableTiles.Concat(unreachablePositionsNW).ToList();


        var possibleMoves = new HashSet<IBoardPosition>(bishopMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board));

        Assert.IsFalse(possibleMoves.Overlaps(unreachableTiles));

    }


}