using Zenject;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleQueenMovesTests : PossibleMovesTestBase
{
    [Test]
    public void OnEmptyBoard_QueenCanMoveAnywhere(
        [Values(0,1,2,3,4,5,6,7)] int x, [Values(0,1,2,3,4,5,6,7)] int y,
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

        var queenGameObject = GetGameObjectAtPosition(x, y);
        var possibleMoves = queenMoveGenerator.GetPossiblePieceMoves(queenGameObject);


    }
}
