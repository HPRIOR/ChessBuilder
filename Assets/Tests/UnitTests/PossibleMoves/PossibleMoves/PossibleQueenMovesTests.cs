﻿using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.PieceState;
using NUnit.Framework;

namespace Tests.UnitTests.PossibleMoves.PossibleMoves
{
    public class PossibleQueenMovesTests : PossibleMovesTestBase
    {
        [Test]
        public void OnEmptyBoard_QueenCanMoveAnywhere(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y,
            [Values(PieceType.WhiteQueen, PieceType.BlackQueen)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);

            var queenMoveGenerator = GetPossibleMoveGenerator(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
            };

            var board = SetUpBoardWith(pieces);

            var possibleMoves =
                queenMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

            var allMovesFromPosition =
                Enum
                    .GetValues(typeof(Direction))
                    .Cast<Direction>()
                    .ToList()
                    .SelectMany(direction =>
                        GetPositionsIncludingAndPassed(
                            Move.In(direction).Add(RelativePositionToTestedPiece(new BoardPosition(x, y))), direction
                        ));

            Assert.AreEqual(allMovesFromPosition.Count(), possibleMoves.Count());
        }

        [Test]
        public void WithOpposingPieceOnMidBoard_QueenCanTakeAndIsBlockedDiagonally(
            [Values(0, 7)] int x, [Values(0, 7)] int y,
            [Values(PieceType.WhiteQueen, PieceType.BlackQueen)]
            PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);

            var queenMoveGenerator = GetPossibleMoveGenerator(pieceType);

            var onWhiteBand = x == 7 && y == 7 || x == 0 && y == 0;
            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(
                    onWhiteBand ? new BoardPosition(3, 3) : new BoardPosition(4, 3))
                )
            };

            var board = SetUpBoardWith(pieces);


            var unreachablePositionsNe = GetPositionsIncludingAndPassed(new BoardPosition(4, 4), Direction.NE);
            var unreachablePositionsSw = GetPositionsIncludingAndPassed(new BoardPosition(2, 2), Direction.SW);
            var unreachablePositionsNw = GetPositionsIncludingAndPassed(new BoardPosition(3, 4), Direction.NW);
            var unreachablePositionsSe = GetPositionsIncludingAndPassed(new BoardPosition(5, 2), Direction.SE);

            IList<IBoardPosition> unreachableTiles = new List<IBoardPosition>();

            if (onWhiteBand)
                if (x >= 4)
                    unreachableTiles = unreachableTiles.Concat(unreachablePositionsSw).ToList();
                else
                    unreachableTiles = unreachableTiles.Concat(unreachablePositionsNe).ToList();
            else if (x <= 3)
                unreachableTiles = unreachableTiles.Concat(unreachablePositionsSe).ToList();
            else
                unreachableTiles = unreachableTiles.Concat(unreachablePositionsNw).ToList();

            var possibleMoves = new HashSet<IBoardPosition>(
                queenMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)),
                    board));

            Assert.IsFalse(possibleMoves.Overlaps(unreachableTiles.Select(RelativePositionToTestedPiece)));
        }


        [Test]
        public void WithOpposingPieceOnMidBoard_QueenCanTakeAndIsBlockedLaterally(
            [Values(0, 1, 2, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 4, 5, 6, 7)] int y,
            [Values(PieceType.WhiteQueen)] PieceType pieceType
        )
        {
            SetTestedPieceColourWith(pieceType);
            var pieces = new List<(PieceType, IBoardPosition)>
            {
                (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, 3))),
                (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(3, y)))
            };

            var board = SetUpBoardWith(pieces);
            var queenMoveGenerator = GetPossibleMoveGenerator(pieceType);

            var unreachableTilesNorth =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(x, 4)), Direction.N)
                    .ToList();
            var unreachableTilesEast =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(4, y)), Direction.E)
                    .ToList();

            var unreachableTilesSouth =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(x, 2)), Direction.S)
                    .ToList();
            var unreachabletilesWest =
                GetPositionsIncludingAndPassed(RelativePositionToTestedPiece(new BoardPosition(2, y)), Direction.W)
                    .ToList();

            var unreachableTiles =
                x < 3
                    ? unreachableTilesEast
                    : unreachabletilesWest
                        .Concat(
                            y < 3
                                ? unreachableTilesNorth
                                : unreachabletilesWest);

            var possibleMoves =
                queenMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);
            var reachableTiles = new HashSet<IBoardPosition>(possibleMoves);
            Assert.IsFalse(reachableTiles.Overlaps(unreachableTiles));
        }
    }
}