using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleKingMoves : IPieceMoveGenerator
{
    private readonly IPositionTranslator _positionTranslator;
    private readonly IBoardEval _boardEval;

    public PossibleKingMoves(IPositionTranslator positionTranslator, IBoardEval boardEval)
    {
        _positionTranslator = positionTranslator;
        _boardEval = boardEval;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
    {
        var potentialMoves = new List<IBoardPosition>();
        var relativePosition = _positionTranslator.GetRelativePosition(originPosition);

        Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList().ForEach(direction =>
            {
                var newPosition = relativePosition.Add(Move.In(direction));
                var newRelativePosition = _positionTranslator.GetRelativePosition(newPosition);
                if (0 > newPosition.X || newPosition.X > 7
                    || 0 > newPosition.Y || newPosition.Y > 7) return;
                var potentialMoveTile = _positionTranslator.GetRelativeTileAt(newPosition, boardState);
                if (_boardEval.OpposingPieceIn(potentialMoveTile) || _boardEval.NoPieceIn(potentialMoveTile))
                    potentialMoves.Add(newRelativePosition);
            });

        return potentialMoves;
    }
}