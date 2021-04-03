using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleRookMoves : IPieceMoveGenerator
{
    private readonly IBoardScanner _boardScanner;
    private readonly IPositionTranslator _positionTranslator;

    public PossibleRookMoves(
        IBoardScanner boardScanner,
        IPositionTranslator boardPositionTranslator)
    {
        _boardScanner = boardScanner;
        _positionTranslator = boardPositionTranslator;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
    {

        var relativePosition = _positionTranslator.GetRelativePosition(originPosition);
        var possibleDirections = new List<Direction>() { Direction.N, Direction.E, Direction.S, Direction.W };

        return possibleDirections.SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition, boardState));
    }
}