using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class BoardScanner : IBoardScanner
{
    private readonly IBoardEval _boardEval;
    private readonly IPositionTranslator _positionTranslator;

    public BoardScanner(
        PieceColour pieceColour, 
        IBoardEvalFactory boardEvalFactory, 
        IPositionTranslatorFactory positionTranslatorFactory)
    {
        _boardEval = boardEvalFactory.Create(pieceColour);
        _positionTranslator = positionTranslatorFactory.Create(pieceColour);
    }
    public IEnumerable<IBoardPosition> ScanIn(Direction direction, IBoardPosition currentPosition)
    {
        var newPosition = currentPosition.Add(Move.In(direction));
        if (PieceCannotMoveTo(newPosition)) 
            return new List<IBoardPosition>();
        if (TileContainsOpposingPieceAt(newPosition)) 
            return new List<IBoardPosition>() { _positionTranslator.GetRelativePosition(newPosition)};
        return ScanIn(direction, newPosition)
            .Concat(new List<IBoardPosition>() { _positionTranslator.GetRelativePosition(newPosition)});
    }

    private bool PieceCannotMoveTo(IBoardPosition boardPosition)
    {
        var x = boardPosition.X; var y = boardPosition.Y;
        return 0 > x || x > 7 || 0 > y || y > 7 || TileContainsFriendlyPieceAt(boardPosition); 
    }

    private bool TileContainsOpposingPieceAt(IBoardPosition boardPosition) =>
        _boardEval.OpposingPieceIn(_positionTranslator.GetRelativeTileAt(boardPosition));

    private bool TileContainsFriendlyPieceAt(IBoardPosition boardPosition) =>
        _boardEval.FriendlyPieceIn(_positionTranslator.GetRelativeTileAt(boardPosition));

    public class Factory : PlaceholderFactory<PieceColour, BoardScanner> { }
}
