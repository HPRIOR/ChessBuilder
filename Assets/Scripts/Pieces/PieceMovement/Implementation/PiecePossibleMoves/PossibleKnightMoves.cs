using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PossibleKnightMoves : IPieceMoveGenerator
{
    private readonly IPositionTranslator _positionTranslator;
    private readonly IBoardEval _boardEval;

    public PossibleKnightMoves(IPositionTranslator positionTranslator, IBoardEval boardEval)
    {
        _positionTranslator = positionTranslator;
        _boardEval = boardEval;
    }

    public IEnumerable<IBoardPosition> GetPossiblePieceMoves(IBoardPosition originPosition, IBoardState boardState)
    {
        Func<(int X, int Y), bool> coordInBounds = 
            coord => 0 <= coord.X || coord.X <= 7 || 0 <= coord.Y || coord.Y <= 7;
        
        return GetMoveCoords(originPosition)
            .Where(coordInBounds)
            .Select(coord => new BoardPosition(coord.X, coord.Y))
            .Select(pos => _positionTranslator.GetRelativePosition(pos));
    }

    private IEnumerable<(int X, int Y)> GetMoveCoords(IBoardPosition boardPosition)
    {
        int x = boardPosition.X; int y = boardPosition.Y;
        var squareXs = new List<int>(){ x + 2, x - 2};
        var squareYs = new List<int>() { y + 2, y - 2};

        var lateralMoves = 
            squareXs.SelectMany(
                x => Enumerable
                        .Range(0,2)
                        .Select(num => num == 0 ? (x, y + 1): (x, y - 1))
                );

        var verticalMoves 
            = squareYs.SelectMany(
                y => Enumerable
                        .Range(0, 2)
                        .Select(num => num == 0 ? (x + 1, y) : (x - 1, y))
                );

        return lateralMoves.Concat(verticalMoves);
    }

}
