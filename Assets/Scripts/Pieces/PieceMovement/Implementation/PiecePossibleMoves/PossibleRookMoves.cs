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

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {
        var pieceComponent = piece.GetComponent<Piece>();
        var originalPosition = pieceComponent.BoardPosition;
        var pieceColour = pieceComponent.Info.PieceColour;

        var relativePosition = _positionTranslator.GetRelativePosition(originalPosition);
        var possibleDirections = new List<Direction>() { Direction.N, Direction.E, Direction.S, Direction.W };

        return possibleDirections.SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition));
    }
}