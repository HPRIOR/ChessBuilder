using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossibleWhitePawnMoves : AbstractPossibleMoveGenerator
{

    public PossibleWhitePawnMoves(IBoardState boardState) : base(boardState){}

    public override IEnumerable<IBoardPosition> PossibleBoardMoves(GameObject piece)
    {
        var originPosition = piece.GetComponent<Piece>().BoardPosition;
        var potentialMoves = new List<IBoardPosition>();
        
        if (originPosition.Y == 7) return potentialMoves; // allow to change piece

        var originX = originPosition.X;
        var originY = originPosition.Y;

        if (GetTileAt(originX, originY + 1).CurrentPiece != null)
            potentialMoves.Add(new BoardPosition(originX, originY + 1));

        
        if (originX > 0)
        {
            var topLeftTile = GetTileAt(originX - 1, originY + 1);
            var topLeftPiece = topLeftTile.CurrentPiece?.GetComponent<Piece>();
            if (topLeftPiece != null && topLeftPiece.PieceColour == PieceColour.Black)
            {
                potentialMoves.Add(new BoardPosition(originX - 1, originY + 1));
            }
        }

        if (originX <= 7)
        {
            var topRightTile = GetTileAt(originX + 1, originY + 1);
            var topRightPiece = topRightTile.CurrentPiece?.GetComponent<Piece>();
            if (topRightPiece != null && topRightPiece.PieceColour == PieceColour.Black)
            {
                potentialMoves.Add(new BoardPosition(originX + 1, originY + 1));
            }

        }
        return potentialMoves;
    }
}

