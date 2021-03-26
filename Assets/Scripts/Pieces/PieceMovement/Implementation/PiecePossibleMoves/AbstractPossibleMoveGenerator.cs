using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AbstractPossibleMoveGenerator : IPieceMoveGenerator
{
    private IBoardState _boardState;
    public AbstractPossibleMoveGenerator(IBoardState boardState)
    {
        _boardState = boardState;
    }
    protected Func<IBoardPosition, ITile> GetTileRetrievingFunctionFor(PieceColour pieceColour)
    {
        if (pieceColour == PieceColour.White)
            return position => _boardState.GetTileAt(position);
        return position => _boardState.GetMirroredTileAt(position);
    }

    protected bool TileContainsOpposingPiece(ITile tile, PieceColour originColour) =>
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour != originColour;

    protected bool TileContainsFreindlyPiece(ITile tile, PieceColour originColour) =>
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour == originColour;

    protected IBoardPosition GetOriginPositionBasedOn(PieceColour pieceColour, IBoardPosition position) =>
        pieceColour == PieceColour.White ? position : new BoardPosition(Math.Abs(position.X - 7), Math.Abs(position.Y - 7));


    public abstract IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece);


    protected IEnumerable<IBoardPosition> ScanIn(
           Direction direction,
           IBoardPosition currentPosition, 
           Func<IBoardPosition, bool> pieceCannotMoveTo,
           Func<IBoardPosition, bool> tileContainsOpposingPieceAt
           )
    {
        var newPosition = currentPosition.Add(Move.In(direction));
        if (pieceCannotMoveTo(newPosition))
            return new List<IBoardPosition>();
        if (tileContainsOpposingPieceAt(newPosition))
            return new List<IBoardPosition>() { newPosition };
        return ScanIn(
            direction, 
            newPosition,
            pieceCannotMoveTo,
            tileContainsOpposingPieceAt   
            )
            .Concat(new List<IBoardPosition>() { newPosition });
    }

}
