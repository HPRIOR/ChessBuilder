using Zenject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.Piece;
using UnityEngine;

[TestFixture]
public class PossibleKingMovesTests : PossibleMovesTestBase
{
    [Test]
    public void OnEmptyBoard_KingCanMoveAroundItself(
        [Values(1, 2, 3, 4, 5, 6)] int x, [Values(1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.WhiteKing, PieceType.BlackKing)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var kingMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y))
        };

        var board = SetUpBoardWith(pieces);

        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);


        Assert.AreEqual(8, possibleMoves.Count());
         
    }

    [Test]
    public void OnEmptyBoard_KingCanMoveInCorners(
        [Values(0, 7)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteKing, PieceType.BlackKing)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var kingMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y))
        };

        var board = SetUpBoardWith(pieces);

        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);

        Assert.AreEqual(3, possibleMoves.Count());
    }

    [Test]
    public void OnEmptyBoard_KingCanMoveOnSides(
        [Values(0, 7)] int x, [Values(1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.WhiteKing, PieceType.BlackKing)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var kingMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        var board =  SetUpBoardWith(pieces);

        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);

        Assert.AreEqual(5, possibleMoves.Count());
    }

    [Test]
    public void OnEmptyBoard_KingCanMoveOnTopAndBottom(
        [Values(1, 2, 3, 4, 5, 6)] int x, [Values(0, 7)] int y,
        [Values(PieceType.WhiteKing, PieceType.BlackKing)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var kingMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        var board = SetUpBoardWith(pieces);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);


        Assert.AreEqual(5, possibleMoves.Count());
    }

    [Test]
    public void SurroundedByEnemies_KingCanTake(
        [Values(1, 2, 3, 4, 5, 6)] int x, [Values(1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.WhiteKing, PieceType.BlackKing)] PieceType pieceType
        )
    {
        SetTestedPieceColourWith(pieceType);
        var kingMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (GetOppositePieceType(pieceType), new BoardPosition(x + 1, y)),
            (GetOppositePieceType(pieceType), new BoardPosition(x + 1, y - 1)),
            (GetOppositePieceType(pieceType), new BoardPosition(x + 1, y + 1)),
            (GetOppositePieceType(pieceType), new BoardPosition(x - 1, y - 1)),
            (GetOppositePieceType(pieceType), new BoardPosition(x - 1, y)),
            (GetOppositePieceType(pieceType), new BoardPosition(x - 1, y + 1)) ,
            (GetOppositePieceType(pieceType), new BoardPosition(x, y + 1)),
            (GetOppositePieceType(pieceType), new BoardPosition(x, y - 1)),
        };

        var board = SetUpBoardWith(pieces);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);

        Assert.AreEqual(8, possibleMoves.Count());
    }

    [Test]
    public void FriendlyPieceBlocksKing(
        [Values(1, 2, 3, 4, 5, 6)] int x, [Values(1, 2, 3, 4, 5, 6)] int y,
        [Values(PieceType.WhiteKing, PieceType.BlackKing)] PieceType pieceType
        )
    {

        SetTestedPieceColourWith(pieceType);
        var kingMoveGenerator = GetPossibleMoveGenerator(pieceType);
        var pieces = new List<(PieceType, IBoardPosition)>()
        {
            (pieceType, new BoardPosition(x, y)),
            (pieceType, new BoardPosition(x + 1, y )),
        };


        var board = SetUpBoardWith(pieces);

        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(new BoardPosition(x, y), board);

        Assert.AreEqual(7, possibleMoves.Count());
    }




}