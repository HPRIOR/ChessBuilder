using Zenject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        var kingGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(kingGameObject);

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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y)))
        };

        SetUpBoardWith(pieces);

        var kingGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(kingGameObject);

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

        SetUpBoardWith(pieces);

        var kingGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(kingGameObject);

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

        SetUpBoardWith(pieces);

        var kingGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(kingGameObject);

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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x + 1, y))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x + 1, y - 1))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x + 1, y + 1))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x - 1, y - 1))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x - 1, y))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x - 1, y + 1))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, y + 1))),
            (GetOppositePieceType(pieceType), RelativePositionToTestedPiece(new BoardPosition(x, y - 1))),
        };

        SetUpBoardWith(pieces);

        var kingGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(kingGameObject);

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
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x, y))),
            (pieceType, RelativePositionToTestedPiece(new BoardPosition(x + 1, y ))),
        };

        SetUpBoardWith(pieces);

        var kingGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = kingMoveGenerator.GetPossiblePieceMoves(kingGameObject);

        Assert.AreEqual(7, possibleMoves.Count());
    }




}