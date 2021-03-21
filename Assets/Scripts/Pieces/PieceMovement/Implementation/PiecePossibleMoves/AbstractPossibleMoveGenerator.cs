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
    protected Func<int, int, ITile> GetTileRetrievingFunctionBasedOn(PieceColour pieceColour)
    {
        if (pieceColour == PieceColour.White)
            return (x, y) => _boardState.GetTileAt(new BoardPosition(x, y));
        return (x, y) => _boardState.GetMirroredTileAt(new BoardPosition(x, y));
    }

    protected bool TileContainsPieceOfOpposingColour(ITile tile, PieceColour originColour) =>
        tile.CurrentPiece is null ?  false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour != originColour;


    protected int GetOriginPositionBasedOn(PieceColour pieceColour, int coord) =>
        pieceColour == PieceColour.White ? coord : Math.Abs(coord - 7);

    public abstract IEnumerable<IBoardPosition> GetPossiblePieceMoves(GameObject piece);
}
