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

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {
        var pieceComponent = piece.GetComponent<Piece>();
        var piecePosition = pieceComponent.BoardPosition;
        var relativePosition = _positionTranslator.GetRelativePosition(piecePosition);

        return Enum
            .GetValues(typeof(Direction))
            .Cast<Direction>()
            .ToList()
            .SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition));
    }
}
