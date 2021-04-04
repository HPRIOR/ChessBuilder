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