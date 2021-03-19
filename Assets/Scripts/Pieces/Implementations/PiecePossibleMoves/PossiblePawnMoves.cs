using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossiblePawnMoves : AbstractPossibleMoveGenerator
{

    public PossiblePawnMoves(IBoardState boardState) : base(boardState){}

    public override IEnumerable<IBoardPosition> GetPossibleBoardMoves(GameObject piece)
    {

        var pieceComponent = piece.GetComponent<Piece>();
        var originPosition = pieceComponent.BoardPosition;
        var pieceColour = pieceComponent.PieceColour;

        var potentialMoves = new List<IBoardPosition>();

        Func<int, int, ITile> GetTileAt = GetTileRetrievingFunctionBasedOn(pieceColour);
        
        var originX = GetOriginPositionBasedOn(pieceColour, originPosition.X);
        var originY = GetOriginPositionBasedOn(pieceColour, originPosition.Y);

        if (originY == 7) return potentialMoves; // allow to change piece

        // tile above is free
        if (GetTileAt(originX, originY + 1).CurrentPiece != null)
            potentialMoves.Add(
                GetTileAt(originX, originY + 1).BoardPosition
                );

        
        if (originX > 0)
        {
            var topLeftTile = GetTileAt(originX - 1, originY + 1);
            if (TileContainsPieceOfOpposingColourOrIsEmpty(topLeftTile, pieceColour))
                potentialMoves.Add(
                    GetTileAt(originX - 1, originY + 1).BoardPosition
                    );
        }

        if (originX <= 7)
        {
            var topRightTile = GetTileAt(originX + 1, originY + 1);
            if (TileContainsPieceOfOpposingColourOrIsEmpty(topRightTile, pieceColour))
                potentialMoves.Add(
                    GetTileAt(originX + 1, originY + 1).BoardPosition
                    );
        }
        return potentialMoves;
    }
}

