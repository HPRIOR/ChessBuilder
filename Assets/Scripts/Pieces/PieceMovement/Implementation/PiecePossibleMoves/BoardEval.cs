using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardEval : IBoardEval
{
    private readonly PieceColour _pieceColour;

    public BoardEval(PieceColour pieceColour)
    {
        _pieceColour = pieceColour;
    }
    public bool FriendlyPieceIn(ITile tile) =>
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour == _pieceColour;

    public bool OpposingPieceIn(ITile tile) => 
        tile.CurrentPiece is null ? false : tile.CurrentPiece.GetComponent<Piece>().Info.PieceColour != _pieceColour;
}
