using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    
    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {
        
        var pieceComponent = piece.GetComponent<Piece>();
        var piecePosition = pieceComponent.BoardPosition;
        var potentialMoves = new List<IBoardPosition>();

        var originPosition = _positionTranslator.GetRelativePosition(piecePosition);
  

        Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList().ForEach(direction => 
            {
                var newPosition = originPosition.Add(Move.In(direction));
                var newRelativePosition = _positionTranslator.GetRelativePosition(newPosition);
                if ( 0 > newPosition.X || newPosition.X > 7 
                    || 0 > newPosition.Y || newPosition.Y > 7 ) return;
                var newTile = _positionTranslator.GetRelativeTileAt(newPosition);
                if (_boardEval.OpposingPieceIn(newTile) || _boardEval.NoPieceIn(newTile)) 
                    potentialMoves.Add(newRelativePosition);
            });

        return potentialMoves;

    }
}
