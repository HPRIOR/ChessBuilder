using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardScanner : IBoardScanner
{
    private readonly IBoardEval _boardEval;
    private readonly IBoardPositionTranslator _boardPositionTranslator;

    public BoardScanner(IBoardEval boardEval, IBoardPositionTranslator boardPositionTranslator)
    {
        _boardEval = boardEval;
        _boardPositionTranslator = boardPositionTranslator;
    }
    public IEnumerable<IBoardPosition> ScanIn(Direction direction, IBoardPosition currentPosition)
    {
        var newPosition = currentPosition.Add(Move.In(direction));
        if (PieceCannotMoveTo(newPosition)) return new List<IBoardPosition>();
        if (TileContainsOpposingPieceAt(newPosition)) return new List<IBoardPosition>() { newPosition };
        return ScanIn(direction, newPosition).Concat(new List<IBoardPosition>() { newPosition });
    }

    private bool PieceCannotMoveTo(IBoardPosition boardPosition)
    {
        var x = boardPosition.X; var y = boardPosition.Y;
        return 0 > x || x > 7 || 0 > y || y > 7 || TileContainsFriendlyPieceAt(boardPosition); 
    }

    private bool TileContainsOpposingPieceAt(IBoardPosition boardPosition) =>
        _boardEval.OpposingPieceIn(_boardPositionTranslator.GetRelativeTileAt(boardPosition));

    private bool TileContainsFriendlyPieceAt(IBoardPosition boardPosition) =>
        _boardEval.FriendlyPieceIn(_boardPositionTranslator.GetRelativeTileAt(boardPosition));
}
