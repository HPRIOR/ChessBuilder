using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossiblePawnMoves : IPieceMoveGenerator
{
    private readonly IPositionTranslator _boardPositionTransator;
    private readonly IBoardEval _boardEval;

    public PossiblePawnMoves(IPositionTranslator boardPositionTranslator, IBoardEval boardEval) 
    {
        _boardPositionTransator = boardPositionTranslator;
        _boardEval = boardEval;
    }
   


    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {

        var pieceComponent = piece.GetComponent<Piece>();
        var piecePosition = pieceComponent.BoardPosition;
        var potentialMoves = new List<IBoardPosition>();

        var originPosition = _boardPositionTransator.GetRelativePosition(piecePosition);

        if (originPosition.Y == 7) return potentialMoves; // allow to change piece

        if (_boardPositionTransator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.N))).CurrentPiece == null)
            potentialMoves.Add(
                _boardPositionTransator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.N))).BoardPosition
                );

        if (originPosition.X > 0)
        {
            var topLeftTile = _boardPositionTransator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW)));
            if (_boardEval.OpposingPieceIn(topLeftTile))
                potentialMoves.Add(
                    _boardPositionTransator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NW))).BoardPosition
                    );
        }

        if (originPosition.X < 7)
        {
            var topRightTile = _boardPositionTransator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE)));
            if (_boardEval.OpposingPieceIn(topRightTile))
                potentialMoves.Add(
                    _boardPositionTransator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.NE))).BoardPosition
                    );
        }

        return potentialMoves;
    }
}

