﻿using System.Collections.Generic;

public class PossiblePawnMoves : IPieceMoveGenerator
{
    private readonly IPositionTranslator _positionTranslator;
    private readonly IBoardEval _boardEval;

    public PossiblePawnMoves(IPositionTranslator boardPositionTranslator, IBoardEval boardEval)
    {
        _positionTranslator = boardPositionTranslator;
        _boardEval = boardEval;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
    {
        var potentialMoves = new List<IBoardPosition>();

        originPosition = _positionTranslator.GetRelativePosition(originPosition);

        if (originPosition.Y == 7) return potentialMoves; // allow to change piece

        if (_positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.N)), boardState).CurrentPiece == PieceType.NullPiece)
            potentialMoves.Add(
                _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.N)))
                );

        if (originPosition.X > 0)
        {
            var topLeftTile = _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW)), boardState);
            if (_boardEval.OpposingPieceIn(topLeftTile))
                potentialMoves.Add(
                    _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NW)))
                    );
        }

        if (originPosition.X < 7)
        {
            var topRightTile = _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE)), boardState);
            if (_boardEval.OpposingPieceIn(topRightTile))
                potentialMoves.Add(
                    _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NE)))
                    );
        }

        return potentialMoves;
    }
}