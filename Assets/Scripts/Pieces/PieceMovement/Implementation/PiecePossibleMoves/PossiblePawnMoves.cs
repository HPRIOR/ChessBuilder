using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossiblePawnMoves : AbstractPossibleMoveGenerator
{

    public PossiblePawnMoves(IBoardState boardState) : base(boardState) { }

    public override IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece)
    {

        var pieceComponent = piece.GetComponent<Piece>();
        var piecePosition = pieceComponent.BoardPosition;
        var pieceColour = pieceComponent.Info.PieceColour;

        var potentialMoves = new List<IBoardPosition>();

        Func<IBoardPosition, ITile> GetTileAt = GetTileRetrievingFunctionFor(pieceColour);

        // make get origin position return a position and use this with MoveInDirection.North
        var originPosition = GetOriginPositionBasedOn(pieceColour, piecePosition);

        if (originPosition.Y == 7) return potentialMoves; // allow to change piece

        if (GetTileAt(originPosition.Add(Move.In(Direction.N))).CurrentPiece == null)
            potentialMoves.Add(
                GetTileAt(originPosition.Add(Move.In(Direction.N))).BoardPosition
                );

        if (originPosition.X > 0)
        {
            var topLeftTile = GetTileAt(originPosition.Add(Move.In(Direction.NW)));
            if (TileContainsOpposingPiece(topLeftTile, pieceColour))
                potentialMoves.Add(
                    GetTileAt(originPosition.Add(Move.In(Direction.NW))).BoardPosition
                    );
        }

        if (originPosition.X < 7)
        {
            var topRightTile = GetTileAt(originPosition.Add(Move.In(Direction.NE)));
            if (TileContainsOpposingPiece(topRightTile, pieceColour))
                potentialMoves.Add(
                    GetTileAt(originPosition.Add(Move.In(Direction.NE))).BoardPosition
                    );
        }

        return potentialMoves;
    }
}

