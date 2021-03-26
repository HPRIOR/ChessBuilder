using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardEvalFactory : IBoardEvalFactory
{
    private readonly BoardEval.Factory _boardEvalFactory;

    public BoardEvalFactory(BoardEval.Factory boardEvalFactory)
    {
        _boardEvalFactory = boardEvalFactory;
    }

    public IBoardEval Create(PieceColour pieceColour) =>
        _boardEvalFactory.Create(pieceColour);
}
