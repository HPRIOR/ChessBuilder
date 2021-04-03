using System.Collections.Generic;

public class PossibleBoardMovesGenerator : IPossibleMovesGenerator
{
    private IPieceMoveGeneratorFactory _pieceMoveGeneratorFactory;

    public PossibleBoardMovesGenerator(IPieceMoveGeneratorFactory pieceMoveGeneratorFactory)
    {
        _pieceMoveGeneratorFactory = pieceMoveGeneratorFactory;
    }

    public IDictionary<IBoardPosition, HashSet<IBoardPosition>> GeneratePossibleMoves(IBoardState boardState)
    {
        var result = new Dictionary<IBoardPosition, HashSet<IBoardPosition>>();
        var board = boardState.Board;
        foreach (var tile in board)
        {
            if (tile.CurrentPiece != PieceType.NullPiece)
            {
                var currentPiece = tile.CurrentPiece;
                var boardPos = tile.BoardPosition;
                var possibleMoves = _pieceMoveGeneratorFactory.GetPossibleMoveGenerator(currentPiece).GetPossiblePieceMoves(boardPos, boardState);
                result.Add(boardPos, new HashSet<IBoardPosition>(possibleMoves));
            }
        }
        //result.ToList().ForEach(x => x.Value.ToList().ForEach(x => Debug.Log(x)));
        return result;
    }
}