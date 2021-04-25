using Zenject;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.Piece;
using UnityEngine;

[TestFixture]
public class PossiblePawnMovesTests : PossibleMovesTestBase
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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        //var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        //Assert.AreEqual(1, possibleMoves.Count());
        //Assert.AreEqual(RelativePositionToTestedPiece(new BoardPosition(x, y + 1)), possibleMoves.First());
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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x + 1, y + 1)))
        };

        SetUpBoardWith(pieces);

        //var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(pawnGameObject);

        //Assert.AreEqual(2, possibleMoves.Count());
        //Assert.Contains(RelativePositionToTestedPiece(new BoardPosition(x + 1, y + 1)), (ICollection)possibleMoves);

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
            (pieceType,  RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece( new BoardPosition(x - 1, y + 1)))
        };

        var board = SetUpBoardWith(pieces);

        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

        Assert.AreEqual(2, possibleMoves.Count());
        Assert.Contains(RelativePositionToTestedPiece( new BoardPosition(x - 1, y + 1)), (ICollection)possibleMoves);
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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (whiteOrBlackPiece, RelativePositionToTestedPiece(new BoardPosition(x, y + 1)))
        };

        var board = SetUpBoardWith(pieces);

        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x + 1, y + 1))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x - 1, y + 1)))
        };

        var board = SetUpBoardWith(pieces);

        var possibleMoves = pawnMoveGenerator.GetPossiblePieceMoves(RelativePositionToTestedPiece(new BoardPosition(x, y)), board);

        Assert.AreEqual(3, possibleMoves.Count());
        Assert.Contains(RelativePositionToTestedPiece(new BoardPosition(x - 1, y + 1)), (ICollection)possibleMoves);
        Assert.Contains(RelativePositionToTestedPiece(new BoardPosition(x + 1, y + 1)), (ICollection)possibleMoves);
    }

}