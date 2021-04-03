using UnityEngine;

/*
 * changes: Move signature change to Func<IBoardState, IBoardPos, IBoardPos, ITile[]> 
 * Don't update pieces on move but produce new board with moved piece types
 * remove undo move from this class as state will just be restored by refering to previous state
 * remove Boardstate from constructor, and pass as argument so it can be applied to arbitrary boards
 * 
 */

public class PieceMover : IPieceMover
{
    public IBoardState Move(IBoardState originalBoard, IBoardPosition from, IBoardPosition destination)
    {
        var newBoard = (IBoardState)originalBoard.Clone();
        var destinationTile = newBoard.GetTileAt(destination);
        var fromTile = newBoard.GetTileAt(from);

        destinationTile.CurrentPiece = originalBoard.GetTileAt(from).CurrentPiece;
        fromTile.CurrentPiece = PieceType.NullPiece;
        return newBoard;
    }
}