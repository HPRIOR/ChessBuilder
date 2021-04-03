using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossibleQueenMoves : IPieceMoveGenerator
{
    private readonly IPositionTranslator _positionTranslator;
    private readonly IBoardScanner _boardScanner;

    public PossibleQueenMoves(IPositionTranslator positionTranslator, IBoardScanner boardScanner)
    {
        _positionTranslator = positionTranslator;
        _boardScanner = boardScanner;
    }
    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
    {
        var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

        return Enum
            .GetValues(typeof(Direction))
            .Cast<Direction>()
            .ToList()
            .SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition, boardState));
    }
}
