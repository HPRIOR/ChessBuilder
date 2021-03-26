using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PossibleRookMoves : IPieceMoveGenerator 
{
    private readonly IBoardScanner _boardScanner;
    private readonly IBoardPositionTranslator _boardPositionTranslator;

    public PossibleRookMoves(
        IBoardScanner boardScanner,
        IBoardPositionTranslator boardPositionTranslator)
    {
        _boardScanner = boardScanner;
        _boardPositionTranslator = boardPositionTranslator;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {
        var pieceComponent = piece.GetComponent<Piece>();
        var originalPosition = pieceComponent.BoardPosition;
        var pieceColour = pieceComponent.Info.PieceColour;

        var relativePosition = _boardPositionTranslator.GetRelativePosition(originalPosition);
        var possibleDirections = new List<Direction>() { Direction.N, Direction.E, Direction.S, Direction.W };

        return possibleDirections.SelectMany(direction => _boardScanner.ScanIn(direction, relativePosition));
    }

}
