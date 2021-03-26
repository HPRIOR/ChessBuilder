using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossiblePawnMoves : IPieceMoveGenerator
{
    private readonly IPositionTranslator _positionTranslator;
    private readonly IBoardEval _boardEval;

    public PossiblePawnMoves(IPositionTranslator boardPositionTranslator, IBoardEval boardEval) 
    {
        _positionTranslator = boardPositionTranslator;
        _boardEval = boardEval;
    }
   
    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {

        var pieceComponent = piece.GetComponent<Piece>();
        var piecePosition = pieceComponent.BoardPosition;
        var potentialMoves = new List<IBoardPosition>();

        var originPosition = _positionTranslator.GetRelativePosition(piecePosition);

        if (originPosition.Y == 7) return potentialMoves; // allow to change piece

        if (_positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.N))).CurrentPiece == null)
            potentialMoves.Add(
                _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.N)))
                );

        if (originPosition.X > 0)
        {
            var topLeftTile = _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW)));
            if (_boardEval.OpposingPieceIn(topLeftTile))
                potentialMoves.Add(
                    _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NW)))
                    );
        }

        if (originPosition.X < 7)
        {
            var topRightTile = _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE)));
            if (_boardEval.OpposingPieceIn(topRightTile))
                potentialMoves.Add(
                    _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.NE)))
                    );
        }

        return potentialMoves;
    }
}

