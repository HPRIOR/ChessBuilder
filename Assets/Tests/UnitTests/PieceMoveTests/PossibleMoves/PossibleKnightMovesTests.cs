using System;
using Zenject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.Piece;
using UnityEngine;

public class PossibleKnightMovesTests : PossibleMovesTestBase
{
    // tests counts on edges and corners of board

    [Test]
    public void OnEmptyBoard_KnightCanMove(
        [Values(2, 3, 4, 5)] int x, [Values(2, 3, 4, 5)] int y,
        [Values(PieceType.WhiteKnight, PieceType.BlackKnight)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        //var possibleMoves = new HashSet<IBoardPosition>(knightMoveGenerator.GetPossiblePieceMoves(knightGameObject));

        //Assert.AreEqual(8, possibleMoves.Count);
    }

    [Test]
    public void OnEmptyBoard_KnightMovesToCorrectPosition(
        [Values(2, 3, 4, 5)] int x, [Values(2, 3, 4, 5)] int y,
        [Values(PieceType.WhiteKnight, PieceType.BlackKnight)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);


        var expectedMoves = new List<IBoardPosition>()
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
        //var possibleMoves = new HashSet<IBoardPosition>(knightMoveGenerator.GetPossiblePieceMoves(knightGameObject));
        //expectedMoves.ForEach(move => Assert.IsTrue(possibleMoves.Contains(move)));
    }
    
    [Test]
    public void WithFriendlyPiece_KnightIsBlocked(
        [Values(2, 3, 4, 5)] int x, [Values(2, 3, 4, 5)] int y,
        [Values(PieceType.WhiteKnight, PieceType.BlackKnight)] PieceType pieceType)
    {
        SetTestedPieceColourWith(pieceType);
        var knightMoveGenerator = GetPossibleMoveGenerator(pieceType);

        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x + 2, y + 1)))
        };

        SetUpBoardWith(pieces);


        var expectedMoves = new List<IBoardPosition>()
        {
            new BoardPosition(x + 2, y - 1),
            new BoardPosition(x - 2, y + 1),
            new BoardPosition(x - 2, y - 1),
            new BoardPosition(x + 1, y + 2),
            new BoardPosition(x - 1, y + 2),
            new BoardPosition(x + 1, y - 2),
            new BoardPosition(x - 1, y - 2)
        };

        var unexpectedMove = new BoardPosition(x + 2, y + 1);
        //var possibleMoves = new HashSet<IBoardPosition>(knightMoveGenerator.GetPossiblePieceMoves(knightGameObject));

        //expectedMoves.ForEach(move => Assert.IsTrue(possibleMoves.Contains(move)));
        //Assert.AreEqual(7, expectedMoves.Count);
        //Assert.IsFalse(possibleMoves.Contains(unexpectedMove));
    }

}
