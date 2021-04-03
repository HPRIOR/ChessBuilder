using Zenject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PossibleQueenMovesTests : PossibleMovesTestBase
{
    [Test]
    public void OnEmptyBoard_QueenCanMoveAnywhere(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
        [Values(PieceType.WhiteQueen, PieceType.BlackQueen)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);

        var queenMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        var queenGameObject = GetPieceTypeAtPosition(x, y);
        //var possibleMoves = queenMoveGenerator.GetPossiblePieceMoves(queenGameObject);

        var allMovesFromPosition =
            Enum
            .GetValues(typeof(Direction))
            .Cast<Direction>()
            .ToList()
            .SelectMany(direction =>
                GetPositionsIncludingAndPassed(
                    Move.In(direction).Add(RelativePositionToTestedPiece(new BoardPosition(x, y))), direction
                ));

        //Assert.AreEqual(allMovesFromPosition.Count(), possibleMoves.Count());
    }

    [Test]
    public void WithOpposingPieceOnMidBoard_QueenCanTakeAndIsBlockedDiagonally(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteQueen, PieceType.BlackQueen)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);

        var queenMoveGenerator = GetPossibleMoveGenerator(pieceType);

        bool onWhiteBand = x == 7 && y == 7 || x == 0 && y == 0;
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(
                onWhiteBand ? new BoardPosition(3,3) : new BoardPosition(4, 3))
            )
        };

        SetUpBoardWith(pieces);

        var queenGameObject = GetPieceTypeAtPosition(x, y);

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

        //var possibleMoves = new HashSet<IBoardPosition>(queenMoveGenerator.GetPossiblePieceMoves(queenGameObject));

        //Assert.IsFalse(possibleMoves.Overlaps(unreachableTiles.Select(RelativePositionToTestedPiece)));
    }


    [Test]
    public void WithOpposingPieceOnMidBoard_QueenCanTakeAndIsBlockedLaterally(
        [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
        [Values(PieceType.WhiteRook, PieceType.BlackRook)] PieceType pieceType
        )
    {
        try
        {
            SetTestedPieceColourWith(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>() {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 3))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(3, y))),
            };

            SetUpBoardWith(pieces);
            var queenMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var queenGameObject = GetPieceTypeAtPosition(x, y);
            //var possibleMoves = queenMoveGenerator.GetPossiblePieceMoves(queenGameObject);

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

            //HashSet<IBoardPosition> reachableTiles = new HashSet<IBoardPosition>(possibleMoves.Select(RelativePositionToTestedPiece));
            //Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }
        catch (PieceSpawnException)
        {
            Debug.Log("Test skipped due to spawn class");
        }
    }
}
