using Assets.Scripts.Models.Piece;
public class PieceMover : IPieceMover
{
    public IBoardState Move(IBoardState originalBoard, IBoardPosition from, IBoardPosition destination)
    {
        var newBoard = (IBoardState)originalBoard.Clone();
        var destinationTile = newBoard.GetTileAt(destination);
        var fromTile = newBoard.GetTileAt(from);

        destinationTile.CurrentPiece = originalBoard.GetTileAt(from).CurrentPiece;
        fromTile.CurrentPiece = new Piece(PieceType.NullPiece);
        return newBoard;
    }
}