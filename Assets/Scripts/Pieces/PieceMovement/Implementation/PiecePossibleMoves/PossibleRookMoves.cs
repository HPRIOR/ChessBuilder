using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleRookMoves : AbstractPossibleMoveGenerator
{
    public PossibleRookMoves(IBoardState boardState) : base(boardState) { }

    public override IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {
        var pieceComponent = piece.GetComponent<Piece>();
        var originalPosition = pieceComponent.BoardPosition;
        var pieceColour = pieceComponent.Info.PieceColour;

        Func<IBoardPosition, ITile> GetTileAt = GetTileRetrievingFunctionFor(pieceColour);

        var originPosition = GetOriginPositionBasedOn(pieceColour, originalPosition);

        Func<IBoardPosition, bool> cannotMoveInDirectionPredicate =
            p =>
            {
                var x = p.X; var y = p.Y;
                return 0 > x || x > 7 || 0 > y || y > 7 || TileContainsFreindlyPiece(GetTileAt(p), pieceColour);
            };

        Func<IBoardPosition, bool> tileContainsOpposingPiece = p => TileContainsOpposingPiece(GetTileAt(p), pieceColour);

        var possibleDirections = new List<Direction>() { Direction.N, Direction.E, Direction.S, Direction.W };

        return possibleDirections
            .SelectMany(direction => 
            ScanIn(
                direction, 
                originPosition, 
                cannotMoveInDirectionPredicate, 
                tileContainsOpposingPiece));
    }

    // this could be simplified using composition
    // methods of another class could be could which would fullfill the function of the abstractpossiblemovesgenerator
    // a constructor argument for this class would take in a piececolour and determine what the function returns 
    // BoardScanner, TileEvaluator

}
