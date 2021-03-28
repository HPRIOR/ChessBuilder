using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleBishopMoves : IPieceMoveGenerator
{
    private readonly IBoardScanner _boardScanner;
    private readonly IPositionTranslator _positionTranslator;

    public PossibleBishopMoves(IBoardScanner boardScanner, IPositionTranslator positionTranslator)
    {
        _boardScanner = boardScanner;
        _positionTranslator = positionTranslator;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {
        var pieceComponent = piece.GetComponent<Piece>();
        var originalPosition = pieceComponent.BoardPosition;
        var pieceColour = pieceComponent.Info.PieceColour;

        var relativePosition = _positionTranslator.GetRelativePosition(originalPosition);
        var possibleDirections = new List<Direction>() { Direction.NE, Direction.NW, Direction.SE, Direction.SW };

        return possibleDirections.SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition));
    }
}
